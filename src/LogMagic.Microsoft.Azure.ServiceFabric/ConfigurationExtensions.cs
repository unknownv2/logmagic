using LogMagic.Microsoft.Azure.ServiceFabric.Enrichers;
using System.Fabric;
using Microsoft.ServiceFabric.Services.Remoting;
using System;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.FabricTransport.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.ServiceFabric.Actors.Runtime;
using LogMagic.Microsoft.Azure.ServiceFabric.Writers;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Actors.Remoting.FabricTransport.Runtime;
using LogMagic.Microsoft.Azure.ServiceFabric.Remoting;


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

      public static ServiceReplicaListener CreateCorrelatingReplicaListener(this StatefulService service, bool switchOperationContext = false)
      {
         IServiceRemotingMessageHandler messageHandler = CreateHandler(service, service.Context, switchOperationContext);

         return new ServiceReplicaListener(ctx =>
            new FabricTransportServiceRemotingListener(ctx, messageHandler));
      }

      public static ServiceReplicaListener CreateCorrelatingActorReplicaListener(this ActorService service, bool switchOperationContext = false)
      {
         IServiceRemotingMessageHandler messageHandler = CreateHandler(service, switchOperationContext);

         return new ServiceReplicaListener(ctx =>
            new FabricTransportActorServiceRemotingListener(ctx, messageHandler, new FabricTransportRemotingListenerSettings()));
      }

      public static ServiceInstanceListener CreateCorrelatingInstanceListener(this StatelessService service, bool switchOperationContext = false)
      {
         IServiceRemotingMessageHandler messageHandler = CreateHandler(service, service.Context, switchOperationContext);

         return new ServiceInstanceListener(ctx =>
            new FabricTransportServiceRemotingListener(ctx, messageHandler));
      }

      private static IServiceRemotingMessageHandler CreateHandler(object serviceInstance, ServiceContext context, bool switchOperationContext)
      {
         if (!(serviceInstance is IService))
         {
            throw new ArgumentException($"service must impelement {typeof(IService).FullName} interface");
         }

         IServiceRemotingMessageHandler
            messageHandler = new CorrelatingRemotingMessageHandler(context, (IService)serviceInstance, switchOperationContext);

         return messageHandler;
      }

      private static IServiceRemotingMessageHandler CreateHandler(ActorService actorService, bool switchOperationContext)
      {
         IServiceRemotingMessageHandler
            messageHandler = new CorrelatingRemotingMessageHandler(actorService, switchOperationContext);

         return messageHandler;
      }
   }
}
