using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LogMagic.FabricTestApp.Interfaces;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.V2.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace LogMagic.FabricTestApp.StatelessSimulator
{
   /// <summary>
   /// An instance of this class is created for each service instance by the Service Fabric runtime.
   /// </summary>
   internal sealed class StatelessSimulator : StatelessService
   {
      private static readonly ILog log = L.G(typeof(StatelessSimulator));

      public StatelessSimulator(StatelessServiceContext context)
         : base(context)
      {
      }

      /// <summary>
      /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
      /// </summary>
      /// <returns>A collection of listeners.</returns>
      protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
      {
         /*return new ServiceInstanceListener[]
         {
            new ServiceInstanceListener(c => new FabricTransportServiceRemotingListener(c, CreateMessageHandler()))
         };*/

         return new ServiceInstanceListener[]
         {
            this.CreateCorrelatingServiceInstanceListener<ISampleService>(new StatelessSimulatorRemotingService(), raiseSummary: RaiseSummary)
         };
      }

      private void RaiseSummary(CallSummary summary)
      {
         log.Trace("call {0} completed in {1}", summary.CallName, summary.DurationTicks, summary.Error);
      }

      /*private IServiceRemotingMessageHandler CreateMessageHandler()
      {
         return new CustomRemotingMessageHandler(Context, new StatelessSimulatorRemotingService());

         //the handler is the "root" object in remoting hierarchy
      }*/

      /// <summary>
      /// This is the main entry point for your service instance.
      /// </summary>
      /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service instance.</param>
      protected override async Task RunAsync(CancellationToken cancellationToken)
      {
         // TODO: Replace the following sample code with your own logic 
         //       or remove this RunAsync override if it's not needed in your service.

         long iterations = 0;

         while (true)
         {
            cancellationToken.ThrowIfCancellationRequested();

            ServiceEventSource.Current.ServiceMessage(this.Context, "Working-{0}", ++iterations);

            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
         }
      }
   }
}