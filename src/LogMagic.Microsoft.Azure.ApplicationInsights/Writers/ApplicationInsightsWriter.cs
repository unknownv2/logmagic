using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LogMagic.Microsoft.Azure.ApplicationInsights.Writers
{
   class ApplicationInsightsWriter : ILogWriter
   {
      private readonly TelemetryClient _telemetryClient;
      private readonly InsightsContext _context;

      public ApplicationInsightsWriter(string instrumentationKey)
      {
         TelemetryConfiguration.Active.InstrumentationKey = instrumentationKey;
         _telemetryClient = new TelemetryClient();
         _context = new InsightsContext(_telemetryClient);
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
