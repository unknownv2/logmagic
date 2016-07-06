using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogMagic.Test
{
   public static class ConsoleEntry
   {
      private static ILog _log = L.G(typeof(ConsoleEntry));

      public static void Main()
      {
         L.Config.WriteToColoredConsole();
         L.NodeId = "localhost";

         _log.D("test");
      }
   }
}
