using LogMagic.Microsoft.Azure.ServiceFabric.Enrichers;
using System.Fabric;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using System;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Client;
using LogMagic.Microsoft.Azure.ServiceFabric.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.FabricTransport.Client;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.FabricTransport.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace LogMagic
{
   public static class ConfigurationExtensions
   {
      public static ILogConfiguration AzureServiceFabricContext(this IEnricherConfiguration configuration, ServiceContext context)
      {
         return configuration.Custom(new ServiceFabricEnricher(context));
      }

      public static ServiceReplicaListener CreateCorrelatingReplicaListener(this StatefulService service)
      {
         if(!(service is IService))
         {
            throw new ArgumentException($"service must impelement {typeof(IService).FullName} interface");
         }

         IServiceRemotingMessageHandler
            messageHandler = new CorrelatingRemotingMessageHandler(service.Context, (IService)service);

         return new ServiceReplicaListener(ctx =>
            new FabricTransportServiceRemotingListener(ctx, messageHandler));
      }

   }
}
