using System;
using System.Fabric;
using System.Threading.Tasks;
using LogMagic.Enrichers;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.Client;

namespace LogMagic.Microsoft.Azure.ServiceFabric.Remoting
{
   class CorrelatingServiceRemotingClient : IServiceRemotingClient, IWrappingClient
   {
      private readonly Uri _serviceUri;
      private readonly IMethodNameProvider _methodNameProvider;

      public CorrelatingServiceRemotingClient(
         IServiceRemotingClient innerClient,
         Uri serviceUri,
         IMethodNameProvider methodNameProvider)
      {
         InnerClient = innerClient;
         _serviceUri = serviceUri;
         _methodNameProvider = methodNameProvider;
      }


      public ResolvedServicePartition ResolvedServicePartition
      {
         get => InnerClient.ResolvedServicePartition;
         set => InnerClient.ResolvedServicePartition = value;
      }

      public string ListenerName
      {
         get => InnerClient.ListenerName;
         set => InnerClient.ListenerName = value;
      }

      public ResolvedServiceEndpoint Endpoint
      {
         get => InnerClient.Endpoint;
         set => InnerClient.Endpoint = value;
      }

      public IServiceRemotingClient InnerClient { get; private set; }

      public Task<byte[]> RequestResponseAsync(ServiceRemotingMessageHeaders messageHeaders, byte[] requestBody)
      {
         return SendAndTrackRequestAsync(messageHeaders, requestBody,
            () => InnerClient.RequestResponseAsync(messageHeaders, requestBody));
      }

      public void SendOneWay(ServiceRemotingMessageHeaders messageHeaders, byte[] requestBody)
      {
         SendAndTrackRequestAsync(messageHeaders, requestBody,
            () =>
            {
               InnerClient.SendOneWay(messageHeaders, requestBody);
               return Task.FromResult<byte[]>(null);
            }).Forget();
      }

      private async Task<byte[]> SendAndTrackRequestAsync(ServiceRemotingMessageHeaders messageHeaders,
         byte[] requestBody, Func<Task<byte[]>> doSendRequest)
      {
         messageHeaders.AddHeader("ivanTest", "ivanValue");

         byte[] result = await doSendRequest().ConfigureAwait(false);

         return result;
      }

      private string GetMethodName(ServiceRemotingMessageHeaders messageHeaders)
      {
         if (messageHeaders.TryGetActorMethodAndInterfaceIds(out int methodId, out int interfaceId))
         {
            return _methodNameProvider.GetMethodName(interfaceId, methodId);
         }

         string methodName = _methodNameProvider.GetMethodName(messageHeaders.InterfaceId, messageHeaders.MethodId);
         if (string.IsNullOrEmpty(methodName)) methodName = messageHeaders.MethodId.ToString();
         return methodName;
      }
   }
}
