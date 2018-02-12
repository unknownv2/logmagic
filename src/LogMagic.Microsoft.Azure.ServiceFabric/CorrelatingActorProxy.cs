using System;
using LogMagic.Microsoft.Azure.ServiceFabric.Remoting;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Services.Remoting.FabricTransport.Client;

namespace LogMagic.Microsoft.Azure.ServiceFabric
{
   public class CorrelatingActorProxy
   {
      public static TActorInterface Create<TActorInterface>(ActorId actorId, Uri serviceUri, string listenerName = null) where TActorInterface : IActor
      {
         var proxyFactory = new CorrelatingActorProxyFactory(callbackClient =>
            new FabricTransportServiceRemotingClientFactory(callbackClient: callbackClient));

         TActorInterface proxy =
            proxyFactory.CreateActorProxy<TActorInterface>(serviceUri, actorId, listenerName);

         return proxy;
      }
   }
}
