using Microsoft.ServiceFabric.Services.Remoting.V2;
using Microsoft.ServiceFabric.Services.Remoting.V2.Client;
using System;
using System.Fabric;
using System.Threading.Tasks;

namespace LogMagic.Microsoft.Azure.ServiceFabric.Remoting
{
   class CorrelatingServiceRemotingClient : IServiceRemotingClient
   {
      private static readonly ILog log = L.G(typeof(CorrelatingServiceRemotingClient));
      private readonly IServiceRemotingClient _inner;
      private readonly RequestEnricher _enricher;

      public CorrelatingServiceRemotingClient(IServiceRemotingClient inner)
      {
         _inner = inner;
         _enricher = new RequestEnricher();
      }

      public IServiceRemotingClient Inner => _inner;

      public ResolvedServicePartition ResolvedServicePartition
      {
         get => _inner.ResolvedServicePartition;
         set => _inner.ResolvedServicePartition = value;
      }

      public string ListenerName
      {
         get => _inner.ListenerName;
         set => _inner.ListenerName = value;
      }

      public ResolvedServiceEndpoint Endpoint
      {
         get => _inner.Endpoint;
         set => _inner.Endpoint = value;
      }

      public async Task<IServiceRemotingResponseMessage> RequestResponseAsync(IServiceRemotingRequestMessage requestRequestMessage)
      {
         _enricher.Enrich(requestRequestMessage);

         using (var time = new TimeMeasure())
         {
            IServiceRemotingResponseMessage response = await _inner.RequestResponseAsync(requestRequestMessage);

            requestRequestMessage.GetHeader().
            return response;
         }
      }

      public void SendOneWay(IServiceRemotingRequestMessage requestMessage)
      {
         _enricher.Enrich(requestMessage);

         _inner.SendOneWay(requestMessage);
      }
   }
}
