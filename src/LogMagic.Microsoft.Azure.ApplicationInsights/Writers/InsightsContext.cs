using LogMagic.Enrichers;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LogMagic.Microsoft.Azure.ApplicationInsights.Writers
{
   class InsightsContext
   {
      private readonly TelemetryClient _client;
      private readonly TelemetryContext _context;

      public InsightsContext(TelemetryClient client)
      {
         _client = client;
         _context = client.Context;
      }

      public void Apply(LogEvent e)
      {
         var tr = new TraceTelemetry(e.FormattedMessage, ToLogSeverity(e.Severity));
         tr.Timestamp = e.EventTime;

         _context.Component.Version = e.UseProperty(KnownProperty.Version, string.Empty);
         _context.Operation.Name = e.UseProperty(KnownProperty.MethodName, string.Empty);
         _context.Cloud.RoleName = e.UseProperty(KnownProperty.NodeName, string.Empty);
         _context.Cloud.RoleInstance = e.UseProperty(KnownProperty.NodeInstanceId, string.Empty);

         if (e.Properties != null)
         {
            tr.Properties.AddRange(e.Properties.ToDictionary(entry => entry.Key, entry => entry.Value?.ToString()));
         }

         _client.TrackTrace(tr);

         if (e.ErrorException != null)
         {
            _client.TrackException(new ExceptionTelemetry(e.ErrorException) { Timestamp = e.EventTime });
         }
      }

      private static SeverityLevel ToLogSeverity(LogSeverity severity)
      {
         switch (severity)
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
   }
}
