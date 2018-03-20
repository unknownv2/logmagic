using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LogMagic.Microsoft.Azure.ApplicationInsights.Writers
{
   //correlation: https://docs.microsoft.com/en-us/azure/application-insights/application-insights-correlation
   class ApplicationInsightsWriter : ILogWriter
   {
      private readonly bool _flushOnWrite;
      private readonly TelemetryClient _telemetryClient;
      private readonly InsightsContext _context;

      public ApplicationInsightsWriter(string instrumentationKey, bool flushOnWrite)
      {
         TelemetryConfiguration.Active.InstrumentationKey = instrumentationKey;

         TelemetryProcessorChainBuilder builder = TelemetryConfiguration.Active.TelemetryProcessorChainBuilder;
         builder.Use(next => new LogMagicTelemetryProcessor(next));
         builder.Build();

         _telemetryClient = new TelemetryClient(TelemetryConfiguration.Active);
         _telemetryClient.InstrumentationKey = instrumentationKey;
         _context = new InsightsContext(_telemetryClient);

         _flushOnWrite = flushOnWrite;
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

         if (_flushOnWrite)
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
