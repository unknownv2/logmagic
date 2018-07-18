using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector;
using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LogMagic.Microsoft.Azure.ApplicationInsights.Writers
{
   //correlation: https://docs.microsoft.com/en-us/azure/application-insights/application-insights-correlation
   class ApplicationInsightsWriter : ILogWriter
   {
      private readonly TelemetryClient _telemetryClient;
      private readonly InsightsContext _context;
      private readonly WriterOptions _options;

      public ApplicationInsightsWriter(string instrumentationKey, WriterOptions options)
      {
         TelemetryConfiguration.Active.InstrumentationKey = instrumentationKey;

         TelemetryProcessorChainBuilder builder = TelemetryConfiguration.Active.TelemetryProcessorChainBuilder;
         QuickPulseTelemetryProcessor quickPulseProcessor = null;
      
         // add our own telemetry processor that can override session based variables
         builder.Use(next => new LogMagicTelemetryProcessor(next));

         // optionally enable QuickPulse
         if(options.EnableQuickPulse)
         {
            builder.Use((next) =>
            {
               quickPulseProcessor = new QuickPulseTelemetryProcessor(next);
               return quickPulseProcessor;
            });
         }

         builder.Build();

         _telemetryClient = new TelemetryClient(TelemetryConfiguration.Active);
         _telemetryClient.InstrumentationKey = instrumentationKey;
         _context = new InsightsContext(_telemetryClient, options);

         if(options.EnableQuickPulse)
         {
            var quickPulse = new QuickPulseTelemetryModule();
            quickPulse.Initialize(TelemetryConfiguration.Active);
            quickPulse.RegisterTelemetryProcessor(quickPulseProcessor);
         }

#if NETFULL
         if (options.CollectPerformanceCounters)
         {
            //optionally enable performance counters collection
            var pcm = new PerformanceCollectorModule();
            //todo: custom counters can be easily added here if required
            //pcm.Counters.Add(new PerformanceCounterCollectionRequest(@"\.NET CLR Memory(LogMagic.Console)\# GC Handles", "GC Handles"));
            pcm.Initialize(TelemetryConfiguration.Active);
         }
#endif

         TelemetryConfiguration.Active.TelemetryInitializers.Add(new OperationTelemetryInitialiser());

         _options = options;
      }

      private void AddTelemetryContext()
      {

      }

      public ApplicationInsightsWriter(TelemetryClient telemetryClient)
      {
         _telemetryClient = telemetryClient;
      }

      public void Write(IEnumerable<LogEvent> events)
      {
         foreach(LogEvent e in events)
         {
            _context.Apply(e);
         }

         if (_options.FlushOnWrite)
         {
            _telemetryClient.Flush();
         }
      }

      public Task WriteAsync(IEnumerable<LogEvent> events)
      {
         Write(events);
         return Task.FromResult(true);
      }

      public void Dispose()
      {
      }

   }
}
