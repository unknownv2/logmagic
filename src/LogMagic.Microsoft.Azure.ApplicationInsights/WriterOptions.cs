namespace LogMagic.Microsoft.Azure.ApplicationInsights
{
   /// <summary>
   /// Writer options
   /// </summary>
   public class WriterOptions
   {
      /// <summary>
      /// Whether to flush the data on each write. Turning this ON slows down logging a LOT,
      /// because it will wait for a successful HTTP submission of the telemetry before moving on.
      /// </summary>
      public bool FlushOnWrite { get; set; } = false;

      /// <summary>
      /// When ON (default) exceptions will be send both as traces and exceptions at the same time.
      /// When OFF exceptions will not be sent to traces.
      /// </summary>
      public bool TraceExceptions { get; set; } = true;

      /// <summary>
      /// Enables Live Stream (AKA QuickPulse). ON by default.
      /// </summary>
      public bool EnableQuickPulse { get; set; } = true;

#if NETFULL
      /// <summary>
      /// When set, collects and sends the most common performance counters. OFF by default due to increased pressure
      /// on outgoing traffic.
      /// </summary>
      public bool CollectPerformanceCounters { get; set; } = false;

      

#endif
   }
}
