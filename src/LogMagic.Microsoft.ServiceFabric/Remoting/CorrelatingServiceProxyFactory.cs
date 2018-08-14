using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Client;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Remoting.V2.Client;
using System;

namespace LogMagic.Microsoft.ServiceFabric.Remoting
{
   class CorrelatingServiceProxyFactory<TServiceInterface> : IServiceProxyFactory
   {
      private readonly ServiceProxyFactory _serviceProxyFactory;

      public CorrelatingServiceProxyFactory(Func<IServiceRemotingCallbackMessageHandler, IServiceRemotingClientFactory> createServiceRemotingClientFactory = null, OperationRetrySettings retrySettings = null, Action<CallSummary> raiseSummary = null, string remoteServiceName = null)
      {
         _serviceProxyFactory = new ServiceProxyFactory(
            (callbackClient) =>
            {
               if (createServiceRemotingClientFactory == null)
               {
                  return new CorrelatingFabricTransportServiceRemotingClientFactory<TServiceInterface>(inner: null, raiseSummary: raiseSummary, remoteServiceName: remoteServiceName);
               }

               IServiceRemotingClientFactory innerClientFactory = createServiceRemotingClientFactory(callbackClient);

               if (innerClientFactory is CorrelatingFabricTransportServiceRemotingClientFactory<TServiceInterface>) return innerClientFactory;

               return new CorrelatingFabricTransportServiceRemotingClientFactory<TServiceInterface>(inner: innerClientFactory, raiseSummary: raiseSummary, remoteServiceName: remoteServiceName);

            },
            retrySettings);
      }

#pragma warning disable CS0693 // Type parameter has the same name as the type parameter from outer type
      public TServiceInterface CreateServiceProxy<TServiceInterface>(Uri serviceUri, ServicePartitionKey partitionKey = null, TargetReplicaSelector targetReplicaSelector = TargetReplicaSelector.Default, string listenerName = null) where TServiceInterface : IService
#pragma warning restore CS0693 // Type parameter has the same name as the type parameter from outer type
      {
         TServiceInterface proxy =
            _serviceProxyFactory.CreateServiceProxy<TServiceInterface>(serviceUri, partitionKey, targetReplicaSelector,
               listenerName);

         MethodResolver.AddMethodsForProxyOrService(proxy.GetType().GetInterfaces(), typeof(IService));

         return proxy;
      }
   }
}
