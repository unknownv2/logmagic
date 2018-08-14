using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LogMagic;
using LogMagic.Enrichers;
using LogMagic.FabricTestApp.ActorSimulator.Interfaces;
using LogMagic.FabricTestApp.Interfaces;
using LogMagic.Microsoft.ServiceFabric.Remoting;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
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
            .WriteTo.AzureApplicationInsights("24703760-10ec-4e0b-b3ee-777f6ea80977")
            .EnrichWith.AzureServiceFabricContext(context)
            .EnrichWith.Constant(KnownProperty.RoleName, "StatefulSimulator")
            .EnrichWith.Constant(KnownProperty.RoleInstance, context.ReplicaOrInstanceId.ToString());

      }

      protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
      {
         //create remote proxy
         ISampleService sample = CreateSampleService();

         return new ServiceReplicaListener[]
         {
            this.CreateCorrelatingReplicaListener<IRootService>(new StatefulSimulatorRemotingService(sample))
         };
      }

      private ISampleService CreateSampleService()
      {
         ISampleService service = CorrelatingProxyFactory.CreateServiceProxy<ISampleService>(
            new Uri("fabric:/LogMagic.FabricTestApp2/LogMagic.FabricTestApp.StatelessSimulator"),
            raiseSummary: RaiseSummary,
            remoteServiceName: "StatelessSimulator");

         return service;
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
         var proxyFactory3 = new ActorProxyFactory();

         //ISampleService service = proxyFactory2.CreateServiceProxy<ISampleService>(
         //   new Uri("fabric:/LogMagic.FabricTestApp2/LogMagic.FabricTestApp.StatelessSimulator"));

         ISampleService service = CorrelatingProxyFactory.CreateServiceProxy<ISampleService>(
            new Uri("fabric:/LogMagic.FabricTestApp2/LogMagic.FabricTestApp.StatelessSimulator"),
            raiseSummary: RaiseSummary,
            remoteServiceName: "StatelessSimulator");

         IActorSimulator actor = CorrelatingProxyFactory.CreateActorProxy<IActorSimulator>(ActorId.CreateRandom());
         //actor = proxyFactory3.CreateActorProxy<IActorSimulator>(ActorId.CreateRandom());

         using (L.Context("param1", "value1"))
         {
            try
            {

               string hey = await service.PingSuccessAsync("hey");

               await actor.SetCountAsync(5, cancellationToken);

               int count = await actor.GetCountAsync(cancellationToken);

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
         //log.Trace("call {0} completed in {1} ticks", summary.CallName, summary.DurationTicks);
      }
   }
}