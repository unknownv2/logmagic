using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;

namespace LogMagic.FabricTestApp.Interfaces
{
   public interface IRootService : IService
   {
      Task TestCall();
   }
}
