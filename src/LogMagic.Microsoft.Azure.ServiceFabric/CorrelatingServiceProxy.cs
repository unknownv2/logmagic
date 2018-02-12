using System;
using LogMagic.Microsoft.Azure.ServiceFabric.Remoting;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Client;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.FabricTransport.Client;

namespace LogMagic.Microsoft.Azure.ServiceFabric
{
   public static class CorrelatingServiceProxy
   {
      public static TServiceInterface Create<TServiceInterface>(
         Uri serviceUri, ServicePartitionKey partitionKey = null, TargetReplicaSelector targetReplicaSelector = TargetReplicaSelector.Default,
         string listenerName = null) where TServiceInterface : IService
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