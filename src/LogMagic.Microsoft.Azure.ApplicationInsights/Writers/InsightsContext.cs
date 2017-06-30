using LogMagic.Enrichers;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Channel;
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
         _context.Component.Version = e.UseProperty(KnownProperty.Version, string.Empty);
         _context.Operation.Name = e.UseProperty(KnownProperty.MethodName, string.Empty);
         _context.Cloud.RoleName = e.UseProperty(KnownProperty.NodeName, string.Empty);
         _context.Cloud.RoleInstance = e.UseProperty(KnownProperty.NodeInstanceId, string.Empty);

         switch(e.EventType)
         {
            case EventType.Dependency:
               ApplyDependency(e);
               break;

            case EventType.ApplicationEvent:
               ApplyEvent(e);
               break;

            case EventType.HandledRequest:
               ApplyRequest(e);
               break;

            default:
               ApplyTrace(e);
               break;
         }

      }

      private void ApplyDependency(LogEvent e)
      {
         string appName = e.UseProperty<string>(KnownProperty.ApplicationName);

         var d = new DependencyTelemetry()
         {
            Type = appName,
            Name = appName,
            Data = e.FormattedMessage,
            Duration = TimeSpan.FromTicks(e.UseProperty<long>(KnownProperty.Duration)),
            Success = e.ErrorException == null,
         };
         Add(d, e.Properties);
         Add(d, e);

         _client.TrackDependency(d);
      }

      private void ApplyEvent(LogEvent e)
      {
         var t = new EventTelemetry
         {
            Name = e.UseProperty<string>(KnownProperty.MethodName)
         };
         Add(t, e);
         Add(t, e.Properties);

         _client.TrackEvent(t);
      }

      private void ApplyTrace(LogEvent e)
      {
         var tr = new TraceTelemetry(e.FormattedMessage, ToLogSeverity(e.Severity));
         Add(tr, e);
         Add(tr, e.Properties);
         _client.TrackTrace(tr);

         if (e.ErrorException != null)
         {
            _client.TrackException(new ExceptionTelemetry(e.ErrorException) { Timestamp = e.EventTime });
         }
      }

      private void ApplyRequest(LogEvent e)
      {
         var tr = new RequestTelemetry
         {
            Name = e.UseProperty<string>(KnownProperty.ApplicationName),
            Duration = TimeSpan.FromTicks(e.UseProperty<long>(KnownProperty.Duration)),
            Success = e.ErrorException == null,
            ResponseCode = e.ErrorException == null ? "200" : e.ErrorException.GetType().Name
         };
         Add(tr, e);
         Add(tr, e.Properties);

         _client.TrackRequest(tr);
      }

      private static void Add(ISupportProperties telemetry, Dictionary<string, object> properties)
      {
         if (properties == null) return;

         telemetry.Properties.AddRange(properties.ToDictionary(entry => entry.Key, entry => entry.Value?.ToString()));
      }

      private static void Add(ITelemetry telemetry, LogEvent e)
      {
         telemetry.Timestamp = e.EventTime;
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
