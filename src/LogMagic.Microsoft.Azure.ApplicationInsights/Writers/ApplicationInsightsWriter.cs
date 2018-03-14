using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LogMagic.Microsoft.Azure.ApplicationInsights.Writers
{
   class ApplicationInsightsWriter : ILogWriter
   {
      private readonly bool _flushOnWrite;
      private readonly TelemetryClient _telemetryClient;
      private readonly InsightsContext _context;

      public ApplicationInsightsWriter(string instrumentationKey, bool flushOnWrite)
      {
         TelemetryConfiguration.Active.InstrumentationKey = instrumentationKey;

         //create custom context to inject important props
         var context = new LogMagicTelemetryContext();
         TelemetryProcessorChainBuilder builder = TelemetryConfiguration.Active.TelemetryProcessorChainBuilder;
         builder.Use(next => context);
         builder.Build();

         _telemetryClient = new TelemetryClient();
         _telemetryClient.InstrumentationKey = instrumentationKey;
         _context = new InsightsContext(_telemetryClient, context);

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
