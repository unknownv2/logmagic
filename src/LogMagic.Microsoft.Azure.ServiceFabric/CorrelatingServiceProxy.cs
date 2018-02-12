using System;
using LogMagic.Microsoft.Azure.ServiceFabric.Remoting.V1;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Client;
using Microsoft.ServiceFabric.Services.Remoting;
#if REMOTING20
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Client;
#else
using Microsoft.ServiceFabric.Services.Remoting.FabricTransport.Client;
#endif

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