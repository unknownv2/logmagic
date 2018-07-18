namespace LogMagic.Microsoft.Azure.ApplicationInsights
{
   /// <summary>
   /// Writer options
   /// </summary>
   public class WriterOptions
   {
      public bool FlushOnWrite { get; set; } = false;

      public bool TraceExceptions { get; set; } = true;

      public bool EnableQuickPulse { get; set; } = false;

#if NETFULL
      /// <summary>
      /// When set, collects and sends the most common performance counters
      /// </summary>
      public bool CollectPerformanceCounters { get; set; } = false;

#endif
   }
}
