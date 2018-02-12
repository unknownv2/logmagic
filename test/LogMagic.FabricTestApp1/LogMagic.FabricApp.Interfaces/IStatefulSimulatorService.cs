using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;

namespace LogMagic.FabricApp.Interfaces
{
   public interface IStatefulSimulatorService : IService
   {
      Task InvokeTest();
   }
}
