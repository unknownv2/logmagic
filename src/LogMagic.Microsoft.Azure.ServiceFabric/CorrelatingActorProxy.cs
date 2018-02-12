using System;
#if REMOTING20
#else
using LogMagic.Microsoft.Azure.ServiceFabric.Remoting.V1;
using Microsoft.ServiceFabric.Services.Remoting.FabricTransport.Client;
#endif
using Microsoft.ServiceFabric.Actors;

namespace LogMagic.Microsoft.Azure.ServiceFabric
{
   public class CorrelatingActorProxy
   {
#if !REMOTING20
      public static TActorInterface Create<TActorInterface>(ActorId actorId, Uri serviceUri, string listenerName = null) where TActorInterface : IActor
      {
         var proxyFactory = new CorrelatingActorProxyFactory(callbackClient =>
            new FabricTransportServiceRemotingClientFactory(callbackClient: callbackClient));

         TActorInterface proxy =
            proxyFactory.CreateActorProxy<TActorInterface>(serviceUri, actorId, listenerName);

         return proxy;
      }
#endif
   }
}
