using System;
using System.Collections.Generic;
using System.Fabric;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Services.Communication.Runtime;

namespace LogMagic.Microsoft.Azure.ServiceFabric.Remoting
{
   public class CorrelatingActorService : ActorService
   {
      public CorrelatingActorService(StatefulServiceContext context, ActorTypeInformation actorTypeInfo, Func<ActorService, ActorId, ActorBase> actorFactory = null, Func<ActorBase, IActorStateProvider, IActorStateManager> stateManagerFactory = null, IActorStateProvider stateProvider = null, ActorServiceSettings settings = null) : base(context, actorTypeInfo, actorFactory, stateManagerFactory, stateProvider, settings)
      {

      }

      protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
      {
         return new[] { this.CreateCorrelatingActorReplicaListener() };
      }
   }
}