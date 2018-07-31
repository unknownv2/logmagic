using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
#if NETFULL
using LogMagic.PerfCounters.Windows;
#endif

namespace LogMagic.PerfCounters
{
   class PerfLoop
   {
      private readonly ILog log;
      private TimeSpan _samplingInterval = TimeSpan.FromSeconds(10);
      private readonly List<IPerformanceCounter> _counters = new List<IPerformanceCounter>();

      public PerfLoop(ILog log)
      {
         this.log = log ?? throw new ArgumentNullException(nameof(log));
#if NETFULL
         _counters.Add(new WindowsPerformanceCounter("Total CPU", "Processor", "% Processor Time", "_Total"));
#endif

         var thread = new Thread(Loop)
         {
            IsBackground = true
         };

         thread.Start();
      }

      private void Loop()
      {
         while(true)
         {
            try
            {
               foreach(IPerformanceCounter ipc in _counters)
               {
                  float value = ipc.GetValue();
                  log.Metric(ipc.Name, value);                  
               }
            }
            catch(Exception ex)
            {
               Trace.WriteLine("fatal error: " + ex);
            }

            Thread.Sleep(_samplingInterval);
         }
      }
   }
}
