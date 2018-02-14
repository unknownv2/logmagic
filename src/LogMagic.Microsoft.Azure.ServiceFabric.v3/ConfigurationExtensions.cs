using LogMagic.Microsoft.Azure.ServiceFabric.Enrichers;
using System.Fabric;
using LogMagic.Microsoft.Azure.ServiceFabric.Writers;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Runtime;
using LogMagic.Microsoft.Azure.ServiceFabric.Remoting;
using Microsoft.ServiceFabric.Actors.Runtime;

namespace LogMagic
{
   public static class ConfigurationExtensions
   {
      /// <summary>
      /// Integrates with Service Fabric Health Reports
      /// </summary>
      /// <param name="configuration"></param>
      /// <param name="context"></param>
      /// <returns></returns>
      public static ILogConfiguration AzureServiceFabricHealthReport(this IWriterConfiguration configuration, ServiceContext context)
      {
         return configuration.Custom(new HealthReportWriter(context));
      }

      /// <summary>
      /// Enriches logging with Service Fabric specific properties
      /// </summary>
      /// <param name="configuration"></param>
      /// <param name="context">Stateful or stateless service context</param>
      /// <returns></returns>
      public static ILogConfiguration AzureServiceFabricContext(this IEnricherConfiguration configuration, ServiceContext context)
      {
         return configuration.Custom(new ServiceFabricEnricher(context));
      }

      public static ServiceInstanceListener CreateCorrelatingServiceInstanceListener(this StatelessService service,
         IService serviceImplementation,
         string listenerName = "")
      {
         var handler = new CorrelatingRemotingMessageHandler(service.Context, serviceImplementation);

         var listener = new ServiceInstanceListener(c => new FabricTransportServiceRemotingListener(c, handler));

         return listener;
      }

      public static ServiceReplicaListener CreateCorrelatingReplicaListener(this StatefulService service,
         IService serviceImplementation,
         string listenerName = "",
         bool listenOnSecondary = false)
      {
         var handler = new CorrelatingRemotingMessageHandler(service.Context, serviceImplementation);

         var listener = new ServiceReplicaListener(c => new FabricTransportServiceRemotingListener(c, handler));

         return listener;
      }
   }
}
