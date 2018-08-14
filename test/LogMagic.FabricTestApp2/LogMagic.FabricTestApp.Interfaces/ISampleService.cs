using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.FabricTransport;
using System;
using System.Threading.Tasks;

namespace LogMagic.FabricTestApp.Interfaces
{
   public interface ISampleService : IService
   {
      Task<string> PingSuccessAsync(string input);

      Task<string> PingFailureAsync(string input);
   }
}
