using System;
using System.Collections.Generic;
using System.Text;

namespace LogMagic.PerfCounters
{
   /// <summary>
   /// Performance counter
   /// </summary>
   public interface IPerformanceCounter
   {
      /// <summary>
      /// Descriptive name, mostly used for logging and nothing else
      /// </summary>
      string Name { get; }

      /// <summary>
      /// Get current counter value
      /// </summary>
      /// <returns></returns>
      float GetValue();
   }
}
