using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchdogService
{
   public interface IServiceConfig
   {
      string AppInsightsKey { get; }
   }
}
