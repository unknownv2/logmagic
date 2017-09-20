using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Client;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.Client;

namespace LogMagic.Microsoft.Azure.ServiceFabric.Remoting
{
   //read more: https://github.com/Microsoft/ApplicationInsights-ServiceFabric

   public class CorrelatingServiceProxyFactory : IServiceProxyFactory
   {
      private readonly ServiceProxyFactory _serviceProxyFactory;
      private readonly MethodNameProvider _methodNameProvider;

      public CorrelatingServiceProxyFactory(
         Func<IServiceRemotingCallbackClient, IServiceRemotingClientFactory> createServiceRemotingClientFactory = null,
         OperationRetrySettings retrySettings = null)

      {
         _methodNameProvider = new MethodNameProvider(true);

         _serviceProxyFactory = new ServiceProxyFactory(
            callbackClient =>
            {
               IServiceRemotingClientFactory innerClientFactory = createServiceRemotingClientFactory(callbackClient);
               return new CorrelatingServiceRemotingClientFactory(innerClientFactory, _methodNameProvider);
            },
            retrySettings);
      }

      public TServiceInterface CreateServiceProxy<TServiceInterface>(Uri serviceUri, ServicePartitionKey partitionKey = null, TargetReplicaSelector targetReplicaSelector = TargetReplicaSelector.Default, string listenerName = null) where TServiceInterface : IService
      {
         TServiceInterface proxy =
            _serviceProxyFactory.CreateServiceProxy<TServiceInterface>(serviceUri, partitionKey, targetReplicaSelector,
               listenerName);

         _methodNameProvider.AddMethodsForProxyOrService(proxy.GetType().GetInterfaces(), typeof(IService));

         return proxy;
      }
   }
}
