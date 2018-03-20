using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LogMagic;
using LogMagic.Enrichers;
using LogMagic.FabricTestApp.Interfaces;
using LogMagic.Microsoft.Azure.ServiceFabric.Remoting;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Client;
using Microsoft.ServiceFabric.Services.Runtime;

namespace StatefulSimulator
{
   /// <summary>
   /// An instance of this class is created for each service replica by the Service Fabric runtime.
   /// </summary>
   internal sealed class StatefulSimulator : StatefulService
   {
      private static readonly ILog log = L.G(typeof(StatefulSimulator));

      public StatefulSimulator(StatefulServiceContext context)
          : base(context)
      {
         L.Config
            .WriteTo.Trace()
            .EnrichWith.AzureServiceFabricContext(context);
      }

      protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
      {
         return new ServiceReplicaListener[0];
      }

      /// <summary>
      /// This is the main entry point for your service replica.
      /// This method executes when this replica of your service becomes primary and has write status.
      /// </summary>
      /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service replica.</param>
      protected override async Task RunAsync(CancellationToken cancellationToken)
      {
         var proxyFactory = new ServiceProxyFactory(c => new FabricTransportServiceRemotingClientFactory());
         var proxyFactory2 = new ServiceProxyFactory(c => new CorrelatingFabricTransportServiceRemotingClientFactory<ISampleService>());

         //ISampleService service = proxyFactory2.CreateServiceProxy<ISampleService>(
         //   new Uri("fabric:/LogMagic.FabricTestApp2/LogMagic.FabricTestApp.StatelessSimulator"));

         ISampleService service = CorrelatingProxyFactory.CreateServiceProxy<ISampleService>(
            new Uri("fabric:/LogMagic.FabricTestApp2/LogMagic.FabricTestApp.StatelessSimulator"),
            raiseSummary: RaiseSummary);

         using (L.Context(new KeyValuePair<string, string>("param1", "value1")))
         {
            try
            {

               string hey = await service.PingSuccessAsync("hey");

               hey = await service.PingFailureAsync("fail");
            }
            catch(Exception ex)
            {
               ex = null;
            }
         }
      }

      private void RaiseSummary(CallSummary summary)
      {
         log.Trace("call {0} completed in {1} ticks", summary.CallName, summary.DurationTicks);
      }
   }
}