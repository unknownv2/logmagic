using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
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
         _telemetryClient = new TelemetryClient();
         _telemetryClient.InstrumentationKey = instrumentationKey;
         _context = new InsightsContext(_telemetryClient);
         _flushOnWrite = flushOnWrite;
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

         _telemetryClient.Flush();
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
