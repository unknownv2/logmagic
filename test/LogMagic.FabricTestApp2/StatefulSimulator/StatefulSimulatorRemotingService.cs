using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LogMagic.FabricTestApp.Interfaces;

namespace StatefulSimulator
{
   public class StatefulSimulatorRemotingService : IRootService
   {
      public StatefulSimulatorRemotingService(ISampleService sample)
      {
         Sample = sample;
      }

      public ISampleService Sample { get; }

      public async Task TestCall()
      {
         await Sample.PingSuccessAsync("test");
      }
   }
}
