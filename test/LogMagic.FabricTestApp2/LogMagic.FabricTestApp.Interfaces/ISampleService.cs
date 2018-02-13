using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Threading.Tasks;

namespace LogMagic.FabricTestApp.Interfaces
{
   public interface ISampleService : IService
   {
      Task<string> GetHelloAsync(string input);
   }
}
