using LogMagic.Microsoft.Azure.ServiceFabric.Enrichers;
using System.Fabric;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using System;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Client;
using LogMagic.Microsoft.Azure.ServiceFabric.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.FabricTransport.Client;

namespace LogMagic
{
   public static class ConfigurationExtensions
   {
      public static ILogConfiguration AzureServiceFabricContext(this IEnricherConfiguration configuration, ServiceContext context)
      {
         return configuration.Custom(new ServiceFabricEnricher(context));
      }

      public static TServiceInterface CreateServiceFabricServiceProxy<TServiceInterface>(
         this ILogConfiguration configuration,
         Uri serviceUri, ServicePartitionKey partitionKey = null, TargetReplicaSelector targetReplicaSelector = TargetReplicaSelector.Default,
         string listenerName = null
         ) where TServiceInterface : IService
      {
         var proxyFactory = new CorrelatingServiceProxyFactory(callbackClient =>
            new FabricTransportServiceRemotingClientFactory(callbackClient: callbackClient));

         TServiceInterface proxy =
            proxyFactory.CreateServiceProxy<TServiceInterface>(serviceUri, partitionKey, targetReplicaSelector,
               listenerName);

         return proxy;
      }

   }
}
