using System;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;

namespace LogMagic.Microsoft.Azure.ServiceFabric.Remoting
{
   class CorrelatingServiceRemotingClientFactory : IServiceRemotingClientFactory
   {
      private readonly IServiceRemotingClientFactory _innerClientFactory;
      private readonly IMethodNameProvider _methodNameProvider;

      public event EventHandler<CommunicationClientEventArgs<IServiceRemotingClient>> ClientConnected;
      public event EventHandler<CommunicationClientEventArgs<IServiceRemotingClient>> ClientDisconnected;

      public CorrelatingServiceRemotingClientFactory(IServiceRemotingClientFactory innerClientFactory,
         IMethodNameProvider methodNameProvider)
      {
         _innerClientFactory = innerClientFactory ?? throw new ArgumentNullException(nameof(innerClientFactory));
         _methodNameProvider = methodNameProvider;

         _innerClientFactory.ClientConnected += ClientConnected;
         _innerClientFactory.ClientDisconnected += ClientDisconnected;
         
      }

      public async Task<IServiceRemotingClient> GetClientAsync(Uri serviceUri, ServicePartitionKey partitionKey, TargetReplicaSelector targetReplicaSelector, string listenerName, OperationRetrySettings retrySettings, CancellationToken cancellationToken)
      {
         IServiceRemotingClient innerClient = await _innerClientFactory
            .GetClientAsync(serviceUri, partitionKey, targetReplicaSelector, listenerName,
            retrySettings, cancellationToken).ConfigureAwait(false);

         return new CorrelatingServiceRemotingClient(innerClient, serviceUri, _methodNameProvider);
      }

      public async Task<IServiceRemotingClient> GetClientAsync(ResolvedServicePartition previousRsp, TargetReplicaSelector targetReplicaSelector, string listenerName, OperationRetrySettings retrySettings, CancellationToken cancellationToken)
      {
         IServiceRemotingClient innerClient = await _innerClientFactory
            .GetClientAsync(previousRsp, targetReplicaSelector, listenerName, retrySettings, cancellationToken)
            .ConfigureAwait(false);

         return new CorrelatingServiceRemotingClient(innerClient, previousRsp.ServiceName, _methodNameProvider);
      }

      public Task<OperationRetryControl> ReportOperationExceptionAsync(IServiceRemotingClient client, ExceptionInformation exceptionInformation, OperationRetrySettings retrySettings, CancellationToken cancellationToken)
      {
         IServiceRemotingClient effectiveClient = (client as IWrappingClient)?.InnerClient ?? client;

         return _innerClientFactory.ReportOperationExceptionAsync(effectiveClient, exceptionInformation, retrySettings,
            cancellationToken);
      }
   }
}
