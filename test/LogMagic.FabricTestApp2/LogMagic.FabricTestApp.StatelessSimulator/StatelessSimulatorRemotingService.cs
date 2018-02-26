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
      public async Task<string> PingFailureAsync(string input)
      {
         throw new ArgumentException(nameof(input));
      }

      public async Task<string> PingSuccessAsync(string input)
      {
         return input + DateTime.UtcNow;
      }
   }
}
