using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.V2;
using Microsoft.ServiceFabric.Services.Remoting.V2.Runtime;
using System;
using System.Fabric;
using System.Threading.Tasks;

namespace LogMagic.FabricTestApp.StatelessSimulator
{
   class CustomRemotingMessageHandler : IServiceRemotingMessageHandler
   {
      private readonly IServiceRemotingMessageHandler _innerHandler;

      public CustomRemotingMessageHandler(ServiceContext serviceContext, IService serviceImplementation)
      {
         _innerHandler = new ServiceRemotingMessageDispatcher(serviceContext, serviceImplementation);
      }

      public IServiceRemotingMessageBodyFactory GetRemotingMessageBodyFactory()
      {
         return _innerHandler.GetRemotingMessageBodyFactory();
      }

      public void HandleOneWayMessage(IServiceRemotingRequestMessage requestMessage)
      {
         _innerHandler.HandleOneWayMessage(requestMessage);
      }

      public Task<IServiceRemotingResponseMessage> HandleRequestResponseAsync(IServiceRemotingRequestContext requestContext, IServiceRemotingRequestMessage requestMessage)
      {
         return _innerHandler.HandleRequestResponseAsync(requestContext, requestMessage);
      }
   }
}
