using System;
using System.Collections.Generic;
using System.Fabric;
using System.Text;
using LogMagic;
using LogMagic.Microsoft.ServiceFabric.Remoting;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Remoting;
using Microsoft.ServiceFabric.Actors.Remoting.V2.FabricTransport.Runtime;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Runtime;

namespace LogMagic.Microsoft.ServiceFabric
{
   /// <summary>
   /// Provides a base class for creating correlating actors for LogMagic
   /// </summary>
   public class CorrelatingActorService : ActorService
   {
      /// <summary>
      /// Instantiates the actor service
      /// </summary>
      public CorrelatingActorService(
         StatefulServiceContext context,
         ActorTypeInformation actorTypeInfo,
         Func<ActorService, ActorId, ActorBase> actorFactory = null,
         Func<ActorBase, IActorStateProvider, IActorStateManager> stateManagerFactory = null,
         IActorStateProvider stateProvider = null,
         ActorServiceSettings settings = null)
         : base(context, actorTypeInfo, actorFactory, stateManagerFactory, stateProvider, settings)
      {
      }

      protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
      {
         //collect base types where actor attribute can be defined
         var typeList = new List<Type> { ActorTypeInformation.ImplementationType };
         typeList.AddRange(ActorTypeInformation.InterfaceTypes);

         //get provider attribute that can override listener endpoints
         ActorRemotingProviderAttribute provider = ActorHelper.GetProvider(typeList);

         //create service remoting listeners the correct way
         var replicaListeners = new List<ServiceReplicaListener>();
         if(Helper.IsEitherRemotingV2(provider.RemotingListenerVersion))
         {
            foreach(KeyValuePair<string, Func<ActorService, IServiceRemotingListener>> remotingListener in provider.CreateServiceRemotingListeners())
            {
               //create our own correlating message handler
               var handler = new CorrelatingRemotingMessageHandler(
                  L.G(GetType()),   //the most upstream type's log
                  this);

               string listenerName = remotingListener.Key;
               Func<ActorService, IServiceRemotingListener> createCommunicationListener = remotingListener.Value;

               /*Func<ActorService, IServiceRemotingListener> debug = a =>
               {
                  IServiceRemotingListener result = createCommunicationListener(a);

                  return result;
               };*/

               var listener = new ServiceReplicaListener(
                  c => new FabricTransportActorServiceRemotingListener(c, handler),
                  listenerName,
                  false);

               /*var listener = new ServiceReplicaListener(
                  //context => createCommunicationListener(this),
                  context => debug(this),
                  listenerName,
                  false);*/

               replicaListeners.Add(listener);
            }
         }

         return replicaListeners;
      }
   }
}