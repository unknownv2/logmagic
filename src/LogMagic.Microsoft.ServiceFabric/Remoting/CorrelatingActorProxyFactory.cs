using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Services.Communication.Client;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.V2.Client;

namespace LogMagic.Microsoft.ServiceFabric.Remoting
{
   class CorrelatingActorProxyFactory<TActorInterface> : IActorProxyFactory where TActorInterface : IActor
   {
      private readonly ActorProxyFactory _actorProxyFactory;

      public CorrelatingActorProxyFactory(
         Func<IServiceRemotingCallbackMessageHandler, IServiceRemotingClientFactory> createServiceRemotingClientFactory = null,
         OperationRetrySettings retrySettings = null,
         Action<CallSummary> raiseSummary = null,
         string remoteServiceName = null)
      {

         _actorProxyFactory = new ActorProxyFactory(
            (callbackClient) =>
            {
               if (createServiceRemotingClientFactory == null)
               {
                  return new CorrelatingFabricTransportServiceRemotingClientFactory<TActorInterface>(inner: null, raiseSummary: raiseSummary, remoteServiceName: remoteServiceName);
               }

               IServiceRemotingClientFactory innerClientFactory = createServiceRemotingClientFactory(callbackClient);

               if (innerClientFactory is CorrelatingFabricTransportServiceRemotingClientFactory<TActorInterface>) return innerClientFactory;

               return new CorrelatingFabricTransportServiceRemotingClientFactory<TActorInterface>(inner: innerClientFactory, raiseSummary: raiseSummary, remoteServiceName: remoteServiceName);

            },
            retrySettings);
      }

#pragma warning disable CS0693 // Type parameter has the same name as the type parameter from outer type
      public TActorInterface CreateActorProxy<TActorInterface>(ActorId actorId, string applicationName = null, string serviceName = null, string listenerName = null) where TActorInterface : IActor
#pragma warning restore CS0693 // Type parameter has the same name as the type parameter from outer type
      {
         TActorInterface proxy = _actorProxyFactory.CreateActorProxy<TActorInterface>(actorId, applicationName, serviceName, listenerName);

         MethodResolver.AddMethodsForProxyOrService(proxy.GetType().GetInterfaces(), typeof(IActor));

         return proxy;
      }

#pragma warning disable CS0693 // Type parameter has the same name as the type parameter from outer type
      public TActorInterface CreateActorProxy<TActorInterface>(Uri serviceUri, ActorId actorId, string listenerName = null) where TActorInterface : IActor
#pragma warning restore CS0693 // Type parameter has the same name as the type parameter from outer type
      {
         TActorInterface proxy = _actorProxyFactory.CreateActorProxy<TActorInterface>(serviceUri, actorId, listenerName);

         MethodResolver.AddMethodsForProxyOrService(proxy.GetType().GetInterfaces(), typeof(IActor));

         return proxy;
      }

      public TServiceInterface CreateActorServiceProxy<TServiceInterface>(Uri serviceUri, ActorId actorId, string listenerName = null) where TServiceInterface : IService
      {
         TServiceInterface proxy = _actorProxyFactory.CreateActorServiceProxy<TServiceInterface>(serviceUri, actorId, listenerName);

         MethodResolver.AddMethodsForProxyOrService(proxy.GetType().GetInterfaces(), typeof(IService));

         return proxy;
      }

      public TServiceInterface CreateActorServiceProxy<TServiceInterface>(Uri serviceUri, long partitionKey, string listenerName = null) where TServiceInterface : IService
      {
         TServiceInterface proxy = _actorProxyFactory.CreateActorServiceProxy<TServiceInterface>(serviceUri, partitionKey, listenerName);

         MethodResolver.AddMethodsForProxyOrService(proxy.GetType().GetInterfaces(), typeof(IService));

         return proxy;
      }
   }
}