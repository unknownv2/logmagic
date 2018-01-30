using LogMagic.Enrichers;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using NetBox.Extensions;
using System;
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

            case EventType.Metric:
               ApplyMetric(e);
               break;

            default:
               ApplyTrace(e);
               break;
         }

      }

      private void ApplyDependency(LogEvent e)
      {
         var d = new DependencyTelemetry()
         {
            Type = e.UseProperty<string>(KnownProperty.DependencyType),
            Name = e.UseProperty<string>(KnownProperty.DependencyName),
            Data = e.UseProperty<string>(KnownProperty.DependencyCommand),
            Duration = TimeSpan.FromTicks(e.UseProperty<long>(KnownProperty.Duration)),
            Success = e.ErrorException == null,
         };
         AddProperties(d, e);
         Add(d, e);

         _client.TrackDependency(d);
      }

      private void ApplyEvent(LogEvent e)
      {
         var t = new EventTelemetry
         {
            Name = e.UseProperty<string>(KnownProperty.EventName)
         };
         Add(t, e);
         AddProperties(t, e);

         _client.TrackEvent(t);
      }

      private void ApplyTrace(LogEvent e)
      {
         var tr = new TraceTelemetry(e.FormattedMessage, GetSeverityLevel(e));
         Add(tr, e);
         AddProperties(tr, e);

         _client.TrackTrace(tr);

         if (e.ErrorException != null)
         {
            var et = new ExceptionTelemetry(e.ErrorException);
            et.Message = e.FormattedMessage;

            Add(et, e);
            AddProperties(et, e);

            _client.TrackException(et);
         }
      }

      private void ApplyRequest(LogEvent e)
      {
         string name = e.UseProperty<string>(KnownProperty.RequestName);

         var tr = new RequestTelemetry
         {
            Name = name,
            Duration = TimeSpan.FromTicks(e.UseProperty<long>(KnownProperty.Duration)),
            Success = e.ErrorException == null,
            ResponseCode = e.ErrorException == null ? "200" : e.ErrorException.GetType().Name
         };
         Add(tr, e);
         AddProperties(tr, e);

         _client.TrackRequest(tr);
      }

      private void ApplyMetric(LogEvent e)
      {
         var t = new MetricTelemetry();
         t.Name = e.UseProperty<string>(KnownProperty.MetricName);
         t.Sum = e.UseProperty<double>(KnownProperty.MetricValue);
         Add(t, e);
         AddProperties(t, e);

         _client.TrackMetric(t);
      }

      //todo:
      //_client.TrackAvailability(null);
      //_client.TrackPageView(null);

      private static void AddProperties(ISupportProperties telemetry, LogEvent e)
      {
         telemetry.Properties.Add("loggerName", e.SourceName);

         if (e.Properties == null) return;

         telemetry.Properties.AddRange(e.Properties.ToDictionary(entry => entry.Key, entry => entry.Value?.ToString()));
      }

      private static void Add(ITelemetry telemetry, LogEvent e)
      {
         telemetry.Timestamp = e.EventTime;
         telemetry.Context.Operation.Id = e.UseProperty<string>(KnownProperty.OperationId);
         telemetry.Context.Operation.ParentId = e.UseProperty<string>(KnownProperty.OperationParentId);
      }

      private static SeverityLevel GetSeverityLevel(LogEvent e)
      {
         LogSeverity sev = e.UseProperty(KnownProperty.Severity, LogSeverity.Information);

         switch (sev)
         {
            case LogSeverity.Verbose:
               return SeverityLevel.Verbose;
            case LogSeverity.Information:
               return SeverityLevel.Information;
            case LogSeverity.Warning:
               return SeverityLevel.Warning;
            case LogSeverity.Error:
               return SeverityLevel.Critical;
            case LogSeverity.Critical:
               return SeverityLevel.Critical;
            default:
               return SeverityLevel.Information;
         }
      }
   }
}
