using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Client;
using Microsoft.ServiceFabric.Services.Remoting;
using System;

namespace LogMagic.Microsoft.ServiceFabric.Remoting
{
   /// <summary>
   /// Helper class to create client proxies faster
   /// </summary>
   public static class CorrelatingProxyFactory
   {
      public static TServiceInterface CreateServiceProxy<TServiceInterface>(
         Uri serviceUri,
         ServicePartitionKey partitionKey = null,
         TargetReplicaSelector targetReplicaSelector = TargetReplicaSelector.Default,
         string listenerName = null,
         Action<CallSummary> raiseSummary = null,
         string remoteServiceName = null) where TServiceInterface : IService
      {
         var proxyFactory = new CorrelatingServiceProxyFactory<TServiceInterface>(raiseSummary: raiseSummary, remoteServiceName: remoteServiceName);

         return proxyFactory.CreateServiceProxy<TServiceInterface>(serviceUri, partitionKey, targetReplicaSelector, listenerName);
      }

      public static TActorInterface CreateActorProxy<TActorInterface>(
         ActorId actorId, string applicationName = null, string serviceName = null, string listenerName = null,
         Action<CallSummary> raiseSummary = null,
         string remoteServiceName = null)
         where TActorInterface : IActor
      {
         var proxyFactory = new CorrelatingActorProxyFactory<TActorInterface>(raiseSummary: raiseSummary, remoteServiceName: remoteServiceName);

         return proxyFactory.CreateActorProxy<TActorInterface>(actorId, applicationName, serviceName, listenerName);
      }

      public static TActorInterface CreateActorProxy<TActorInterface>(
         Uri serviceUri, ActorId actorId, string listenerName = null,
         Action<CallSummary> raiseSummary = null,
         string remoteServiceName = null)
         where TActorInterface : IActor
      {
         var proxyFactory = new CorrelatingActorProxyFactory<TActorInterface>(raiseSummary: raiseSummary, remoteServiceName: remoteServiceName);

         return proxyFactory.CreateActorProxy<TActorInterface>(serviceUri, actorId, listenerName);
      }
   }
}
