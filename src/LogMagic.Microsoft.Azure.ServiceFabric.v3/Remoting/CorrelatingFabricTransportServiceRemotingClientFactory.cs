using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Client;
using Microsoft.ServiceFabric.Services.Remoting.FabricTransport;
using Microsoft.ServiceFabric.Services.Remoting.V2;
using Microsoft.ServiceFabric.Services.Remoting.V2.Client;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Client;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LogMagic.Microsoft.Azure.ServiceFabric.Remoting
{
   public class CorrelatingFabricTransportServiceRemotingClientFactory : IServiceRemotingClientFactory
   {
      private readonly IServiceRemotingClientFactory _inner;
      private readonly Action<CallSummary> _raiseSummary;
      private readonly string _remoteServiceName;

      public event EventHandler<CommunicationClientEventArgs<IServiceRemotingClient>> ClientConnected;
      public event EventHandler<CommunicationClientEventArgs<IServiceRemotingClient>> ClientDisconnected;

      public CorrelatingFabricTransportServiceRemotingClientFactory(
         FabricTransportRemotingSettings remotingSettings = null,
         IServiceRemotingCallbackMessageHandler remotingCallbackMessageHandler = null,
         IServicePartitionResolver servicePartitionResolver = null,
         IEnumerable<IExceptionHandler> exceptionHandlers = null,
         string traceId = null,
         IServiceRemotingMessageSerializationProvider serializationProvider = null,
         Action<CallSummary> raiseSummary = null,
         string remoteServiceName = null) :
         this(remotingSettings, remotingCallbackMessageHandler, servicePartitionResolver, exceptionHandlers, traceId, serializationProvider, null, raiseSummary)
      {

      }

      internal CorrelatingFabricTransportServiceRemotingClientFactory(
         FabricTransportRemotingSettings remotingSettings = null,
         IServiceRemotingCallbackMessageHandler remotingCallbackMessageHandler = null,
         IServicePartitionResolver servicePartitionResolver = null,
         IEnumerable<IExceptionHandler> exceptionHandlers = null,
         string traceId = null,
         IServiceRemotingMessageSerializationProvider serializationProvider = null,
         IServiceRemotingClientFactory inner = null,
         Action<CallSummary> raiseSummary = null,
         string remoteServiceName = null)
      {
         if (inner == null)
         {
            inner = new FabricTransportServiceRemotingClientFactory(
               remotingSettings,
               remotingCallbackMessageHandler,
               servicePartitionResolver,
               exceptionHandlers,
               traceId,
               serializationProvider);
         }

         _inner = inner;
         _raiseSummary = raiseSummary;
         _remoteServiceName = remoteServiceName ?? "unknown";
      }

      public async Task<IServiceRemotingClient> GetClientAsync(Uri serviceUri, ServicePartitionKey partitionKey, TargetReplicaSelector targetReplicaSelector, string listenerName, OperationRetrySettings retrySettings, CancellationToken cancellationToken)
      {
         IServiceRemotingClient inner = await _inner.GetClientAsync(
            serviceUri, partitionKey, targetReplicaSelector, listenerName, retrySettings, cancellationToken);

         return new CorrelatingServiceRemotingClient(inner, _raiseSummary, _remoteServiceName);
      }

      public async Task<IServiceRemotingClient> GetClientAsync(ResolvedServicePartition previousRsp, TargetReplicaSelector targetReplicaSelector, string listenerName, OperationRetrySettings retrySettings, CancellationToken cancellationToken)
      {
         IServiceRemotingClient inner = await _inner.GetClientAsync(
            previousRsp, targetReplicaSelector, listenerName, retrySettings, cancellationToken);

         return new CorrelatingServiceRemotingClient(inner, _raiseSummary, _remoteServiceName);
      }

      public IServiceRemotingMessageBodyFactory GetRemotingMessageBodyFactory()
      {
         return _inner.GetRemotingMessageBodyFactory();
      }

      public async Task<OperationRetryControl> ReportOperationExceptionAsync(IServiceRemotingClient client, ExceptionInformation exceptionInformation, OperationRetrySettings retrySettings, CancellationToken cancellationToken)
      {
         if(client is CorrelatingServiceRemotingClient correlatingClient)
         {
            client = correlatingClient.Inner;
         }

         return await _inner.ReportOperationExceptionAsync(client, exceptionInformation, retrySettings, cancellationToken);
      }
   }
}
