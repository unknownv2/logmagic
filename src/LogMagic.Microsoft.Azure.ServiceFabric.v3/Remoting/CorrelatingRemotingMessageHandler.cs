using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.V2;
using Microsoft.ServiceFabric.Services.Remoting.V2.Runtime;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LogMagic.Microsoft.Azure.ServiceFabric.Remoting
{
   class CorrelatingRemotingMessageHandler : IServiceRemotingMessageHandler
   {
      private static readonly Encoding Enc = Encoding.UTF8;
      private readonly IServiceRemotingMessageHandler _innerHandler;
      private static FieldInfo getHeadersField;

      public CorrelatingRemotingMessageHandler(ServiceContext serviceContext, IService serviceImplementation)
      {
         _innerHandler = new ServiceRemotingMessageDispatcher(serviceContext, serviceImplementation);

         MethodResolver.AddMethodsForProxyOrService(serviceImplementation.GetType().GetInterfaces(), typeof(IService));
      }

      public IServiceRemotingMessageBodyFactory GetRemotingMessageBodyFactory()
      {
         return _innerHandler.GetRemotingMessageBodyFactory();
      }

      public void HandleOneWayMessage(IServiceRemotingRequestMessage requestMessage)
      {
         _innerHandler.HandleOneWayMessage(requestMessage);
      }

      public async Task<IServiceRemotingResponseMessage> HandleRequestResponseAsync(
         IServiceRemotingRequestContext requestContext,
         IServiceRemotingRequestMessage requestMessage)
      {
         Dictionary<string, string> context = ExtractContextProperties(requestMessage);

         if (context == null)
         {
            return await _innerHandler.HandleRequestResponseAsync(requestContext, requestMessage);
         }
         else
         {
            using (L.Context(context))
            {
               return await _innerHandler.HandleRequestResponseAsync(requestContext, requestMessage);
            }
         }
      }

      private Dictionary<string, string> ExtractContextProperties(IServiceRemotingRequestMessage message)
      {
         IServiceRemotingRequestMessageHeader headers = message.GetHeader();

         //DANGER!!! this is using reflection to get to internal dictionary of headers collection.
         if (getHeadersField == null)
         {
            getHeadersField = headers.GetType()
               .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
               .First(f => f.Name == "headers");
         }
         var headersCollection = getHeadersField.GetValue(headers) as Dictionary<string, byte[]>;

         if (headersCollection == null) return null;

         return headersCollection.ToDictionary(
            e => e.Key,
            e => e.Value == null ? null : Enc.GetString(e.Value));
      }
   }
}
