using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogMagic.Receivers;
using LogMagic.Writers;

namespace LogMagic
{
   public static class WriterExtensions
   {
      public static ILogConfiguration WriteToConsole(this ILogConfiguration configuration)
      {
         return configuration.AddWriter(new ConsoleLogWriter());
      }

      public static ILogConfiguration WriteToColoredConsole(this ILogConfiguration configuration)
      {
         return configuration.AddWriter(new PoshConsoleLogWriter());
      }

      public static ILogConfiguration WriteToTrace(this ILogConfiguration configuration)
      {
         return configuration.AddWriter(new TraceLogWriter());
      }

      public static ILogConfiguration WriteToFile(this ILogConfiguration configuration, string fileName)
      {
         return configuration.AddWriter(new FileLogWriter(fileName));
      }
   }
}
