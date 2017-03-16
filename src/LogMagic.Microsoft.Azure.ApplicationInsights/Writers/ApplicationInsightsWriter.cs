using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogMagic.Microsoft.Azure.ApplicationInsights.Writers
{
   class ApplicationInsightsWriter : ILogWriter
   {
      private readonly TelemetryClient _telemetryClient;

      public ApplicationInsightsWriter(string instrumentationKey)
      {
         TelemetryConfiguration.Active.InstrumentationKey = instrumentationKey;
         _telemetryClient = new TelemetryClient();
         _telemetryClient.Context.Cloud.RoleName = "logmagic";
         _telemetryClient.Context.Device.OperatingSystem = ".net core";
      }

      public ApplicationInsightsWriter(TelemetryClient telemetryClient)
      {
         _telemetryClient = telemetryClient;
      }

      public void Write(IEnumerable<LogEvent> events)
      {
         foreach(LogEvent e in events)
         {
            var tr = new TraceTelemetry(e.FormattedMessage, ToLogSeverity(e.Severity));
            tr.Timestamp = e.EventTime;

            if(e.Properties != null)
            {
               tr.Properties.AddRange(e.Properties.ToDictionary(entry => entry.Key, entry => entry.Value?.ToString()));
            }

            _telemetryClient.TrackTrace(tr);

            if (e.ErrorException != null)
            {
               _telemetryClient.TrackException(new ExceptionTelemetry(e.ErrorException) { Timestamp = e.EventTime });
            }
         }

         _telemetryClient.Flush();
      }

      public Task WriteAsync(IEnumerable<LogEvent> events)
      {
         Write(events);
         return Task.FromResult(true);
      }

      private SeverityLevel ToLogSeverity(LogSeverity severity)
      {
         switch(severity)
         {
            case LogSeverity.Debug:
               return SeverityLevel.Verbose;
            case LogSeverity.Info:
               return SeverityLevel.Information;
            case LogSeverity.Warning:
               return SeverityLevel.Warning;
            case LogSeverity.Error:
               return SeverityLevel.Error;
            default:
               return SeverityLevel.Error;
         }
      }

      public void Dispose()
      {
      }

   }
}
