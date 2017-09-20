using Microsoft.ServiceFabric.Services.Remoting.Client;

namespace LogMagic.Microsoft.Azure.ServiceFabric.Remoting
{
   interface IWrappingClient
   {
      IServiceRemotingClient InnerClient { get; }
   }
}
