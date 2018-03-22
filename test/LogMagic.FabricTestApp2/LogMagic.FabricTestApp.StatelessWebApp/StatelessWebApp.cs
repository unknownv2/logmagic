using System;
using System.Collections.Generic;
using System.Fabric;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LogMagic.Enrichers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace LogMagic.FabricTestApp.StatelessWebApp
{
   /// <summary>
   /// The FabricRuntime creates an instance of this class for each service type instance. 
   /// </summary>
   internal sealed class StatelessWebApp : StatelessService
   {
      public StatelessWebApp(StatelessServiceContext context)
          : base(context)
      {
         L.Config
            .WriteTo.Trace()
            .WriteTo.AzureApplicationInsights("24703760-10ec-4e0b-b3ee-777f6ea80977")
            .EnrichWith.AzureServiceFabricContext(context)
            .EnrichWith.Constant(KnownProperty.RoleName, "Frontend Web API")
            .EnrichWith.Constant(KnownProperty.RoleInstance, context.ReplicaOrInstanceId.ToString());
      }

      /// <summary>
      /// Optional override to create listeners (like tcp, http) for this service instance.
      /// </summary>
      /// <returns>The collection of listeners.</returns>
      protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
      {
         return new ServiceInstanceListener[]
         {
                new ServiceInstanceListener(serviceContext =>
                    new KestrelCommunicationListener(serviceContext, "ServiceEndpoint", (url, listener) =>
                    {
                        ServiceEventSource.Current.ServiceMessage(serviceContext, $"Starting Kestrel on {url}");

                        return new WebHostBuilder()
                                    .UseKestrel()
                                    .ConfigureServices(
                                        services => services
                                            .AddSingleton<StatelessServiceContext>(serviceContext))
                                    .UseContentRoot(Directory.GetCurrentDirectory())
                                    .UseStartup<Startup>()
                                    .UseServiceFabricIntegration(listener, ServiceFabricIntegrationOptions.None)
                                    .UseUrls(url)
                                    .Build();
                    }))
         };
      }
   }
}