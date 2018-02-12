#if REMOTING20
using Microsoft.ServiceFabric.Services.Remoting.V2.Client;
#else
using Microsoft.ServiceFabric.Services.Remoting.Client;
#endif

namespace LogMagic.Microsoft.Azure.ServiceFabric.Remoting
{
   interface IWrappingClient
   {
      IServiceRemotingClient InnerClient { get; }
   }
}
