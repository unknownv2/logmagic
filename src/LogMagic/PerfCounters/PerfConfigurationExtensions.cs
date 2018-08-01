using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
#if NETFULL
using LogMagic.PerfCounters.Windows;
#endif

namespace LogMagic
{
   /// <summary>
   /// Performance counters configuration extensions
   /// </summary>
   public static class PerfConfigurationExtensions
   {
      /// <summary>
      /// Add default platform performance counters
      /// </summary>
      public static ILogConfiguration PlatformDefault(this IPerformanceCounterConfiguration configuration)
      {
#if NETFULL
         configuration.WindowsCounter("Total CPU", "Processor", "% Processor Time", "_Total");

         string processName = Process.GetCurrentProcess().ProcessName;
         configuration.WindowsCounter("Process CPU", "Process", "% Processor Time", processName);
#endif

         return configuration.Custom(null);
      }

#if NETFULL
      /// <summary>
      /// Windows based performance counter
      /// </summary>
      public static ILogConfiguration WindowsCounter(this IPerformanceCounterConfiguration configuration,
         string name, string categoryName, string counterName, string instanceName)
      {
         return configuration.Custom(new WindowsPerformanceCounter(name, categoryName, counterName, instanceName));
      }
#endif

   }
}
