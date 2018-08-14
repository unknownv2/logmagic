using LogMagic.FabricTestApp.Interfaces;
using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LogMagic.FabricTestApp.StatelessSimulator
{
   class StatelessSimulatorRemotingService : ISampleService
   {
      public Task<string> PingFailureAsync(string input)
      {
         throw new ArgumentException(nameof(input));
      }

      public Task<string> PingSuccessAsync(string input)
      {
         return Task.FromResult(input + DateTime.UtcNow);
      }
   }
}
