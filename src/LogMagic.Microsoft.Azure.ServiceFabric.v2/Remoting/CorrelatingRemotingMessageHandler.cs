using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using LogMagic.Enrichers;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Remoting.Runtime;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;

namespace LogMagic.Microsoft.Azure.ServiceFabric.Remoting
{
   /// <summary>
   /// Service remoting handler that wraps a service and parses correlation id and context, if they have been passed by the caller as
   /// message headers. This allows traces the client and the service to log traces with the same relevant correlation id and context.
   /// </summary>
   public class CorrelatingRemotingMessageHandler : IServiceRemotingMessageHandler
   {
      private Lazy<DataContractSerializer> _baggageSerializer;

      private IServiceRemotingMessageHandler _innerHandler;
      //private TelemetryClient telemetryClient;
      private MethodNameProvider _methodNameProvider;
      private bool _isActorService;
      private readonly bool _switchOperationContext;

      /// <summary>
      /// Initializes the <see cref="CorrelatingRemotingMessageHandler"/> object. It wraps the given service for all the core
      /// operations for servicing the request.
      /// </summary>
      /// <param name="service">The service whose remoting messages this handler should handle.</param>
      /// <param name="serviceContext">The context object for the service.</param>
      public CorrelatingRemotingMessageHandler(ServiceContext serviceContext, IService service, bool switchOperationContext)
      {
         this.InitializeCommonFields();
         this._innerHandler = new ServiceRemotingDispatcher(serviceContext, service);

         // Populate our method name provider with methods from the IService interfaces
         this._methodNameProvider.AddMethodsForProxyOrService(service.GetType().GetInterfaces(), typeof(IService));
         _switchOperationContext = switchOperationContext;
      }

      /// <summary>
      /// Initializes the <see cref="CorrelatingRemotingMessageHandler"/> object. It wraps the given actor service for all the core
      /// operations for servicing the request.
      /// </summary>
      /// <param name="actorService">The actor service whose remoting messages this handler should handle.</param>
      public CorrelatingRemotingMessageHandler(ActorService actorService, bool switchOperationContext)
      {
         InitializeCommonFields();
         _innerHandler = new ActorServiceRemotingDispatcher(actorService);
         _isActorService = true;
         _switchOperationContext = switchOperationContext;

         // Populate our method name provider with methods from the ActorService interfaces, and the Actor interfaces
         _methodNameProvider.AddMethodsForProxyOrService(actorService.GetType().GetInterfaces(), typeof(IService));
         _methodNameProvider.AddMethodsForProxyOrService(actorService.ActorTypeInformation.InterfaceTypes, typeof(IActor));
      }

      /// <summary>
      /// Handles a one way message from the client. It consumes the correlation id and context from the message headers, if any.
      /// </summary>
      /// <param name="requestContext">Request context - contains additional information about the request.</param>
      /// <param name="messageHeaders">Request message headers.</param>
      /// <param name="requestBody">Request message body.</param>
      public void HandleOneWay(IServiceRemotingRequestContext requestContext, ServiceRemotingMessageHeaders messageHeaders, byte[] requestBody)
      {
         HandleAndTrackRequestAsync(messageHeaders, () =>
         {
            _innerHandler.HandleOneWay(requestContext, messageHeaders, requestBody);
            return Task.FromResult<byte[]>(null);
         }).Forget();
      }

      /// <summary>
      /// Handles a message from the client that requires a response from the service. It consumes the correlation id and
      /// context from the message headers, if any.
      /// </summary>
      /// <param name="requestContext">Request context - contains additional information about the request.</param>
      /// <param name="messageHeaders">Request message headers.</param>
      /// <param name="requestBody">Request message body</param>
      /// <returns></returns>
      public Task<byte[]> RequestResponseAsync(IServiceRemotingRequestContext requestContext, ServiceRemotingMessageHeaders messageHeaders, byte[] requestBody)
      {
         return HandleAndTrackRequestAsync(messageHeaders, () => _innerHandler.RequestResponseAsync(requestContext, messageHeaders, requestBody));
      }

      private void InitializeCommonFields()
      {
         //this.telemetryClient = new TelemetryClient();
         _baggageSerializer = new Lazy<DataContractSerializer>(() => new DataContractSerializer(typeof(IEnumerable<KeyValuePair<string, string>>)));
         _methodNameProvider = new MethodNameProvider(false /* threadSafe */);
      }

      private async Task<byte[]> HandleAndTrackRequestAsync(ServiceRemotingMessageHeaders messageHeaders, Func<Task<byte[]>> doHandleRequest)
      {
         Dictionary<string, string> contextProperties = CorrelationHeader.ParseHeaders(messageHeaders, out string operationId);

         // Create and prepare activity and RequestTelemetry objects to track this request.
         //RequestTelemetry rt = new RequestTelemetry();

         /*if (messageHeaders.TryGetHeaderValue(ServiceRemotingLoggingStrings.ParentIdHeaderName, out string parentId))
         {
            rt.Context.Operation.ParentId = parentId;
            rt.Context.Operation.Id = GetOperationId(parentId);
         }*/

         // Do our best effort in setting the request name.
         string methodName = null;

         if (this._isActorService && messageHeaders.TryGetActorMethodAndInterfaceIds(out int methodId, out int interfaceId))
         {
            methodName = this._methodNameProvider.GetMethodName(interfaceId, methodId);

            // Weird case, we couldn't find the method in the map. Just use the numerical id as the method name
            if (string.IsNullOrEmpty(methodName))
            {
               methodName = methodId.ToString();
            }
         }
         else
         {
            methodName = this._methodNameProvider.GetMethodName(messageHeaders.InterfaceId, messageHeaders.MethodId);

            // Weird case, we couldn't find the method in the map. Just use the numerical id as the method name
            if (string.IsNullOrEmpty(methodName))
            {
               methodName = messageHeaders.MethodId.ToString();
            }
         }

         //rt.Name = methodName;

         /*if (messageHeaders.TryGetHeaderValue(ServiceRemotingLoggingStrings.CorrelationContextHeaderName, out byte[] correlationBytes))
         {
            var baggageBytesStream = new MemoryStream(correlationBytes, writable: false);
            var dictionaryReader = XmlDictionaryReader.CreateBinaryReader(baggageBytesStream, XmlDictionaryReaderQuotas.Max);
            var baggage = this.baggageSerializer.Value.ReadObject(dictionaryReader) as IEnumerable<KeyValuePair<string, string>>;

            foreach (KeyValuePair<string, string> pair in baggage)
            {
               rt.Context.Properties.Add(pair.Key, pair.Value);
            }
         }*/

         // Call StartOperation, this will create a new activity with the current activity being the parent.
         // Since service remoting doesn't really have an URL like HTTP URL, we will do our best approximate that for
         // the Name, Type, Data, and Target properties
         //var operation = telemetryClient.StartOperation<RequestTelemetry>(rt);

         string currentOperationId = _switchOperationContext ? Guid.NewGuid().ToString() : operationId;

         using (L.Context(contextProperties))
         using (L.Context(KnownProperty.OperationId, currentOperationId))
         {
            try
            {
               byte[] result = await doHandleRequest().ConfigureAwait(false);
               return result;
            }
            catch (Exception e)
            {
               //telemetryClient.TrackException(e);
               //operation.Telemetry.Success = false;
               throw;
            }
            finally
            {
               // Stopping the operation, this will also pop the activity created by StartOperation off the activity stack.
               //telemetryClient.StopOperation(operation);
            }
         }
      }

      /// <summary>
      /// Gets the operation Id from the request Id: substring between '|' and first '.'.
      /// </summary>
      /// <param name="id">Id to get the operation id from.</param>
      private static string GetOperationId(string id)
      {
         // id MAY start with '|' and contain '.'. We return substring between them
         // ParentId MAY NOT have hierarchical structure and we don't know if initially rootId was started with '|',
         // so we must NOT include first '|' to allow mixed hierarchical and non-hierarchical request id scenarios
         int rootEnd = id.IndexOf('.');
         if (rootEnd < 0)
         {
            rootEnd = id.Length;
         }

         int rootStart = id[0] == '|' ? 1 : 0;
         return id.Substring(rootStart, rootEnd - rootStart);
      }
   }

}