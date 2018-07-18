using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LogMagic.Enrichers;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace LogMagic.Microsoft.Azure.ApplicationInsights.Writers
{
   class LogMagicTelemetryProcessor : ITelemetryProcessor
   {
      private readonly ITelemetryProcessor _next;

      public LogMagicTelemetryProcessor(ITelemetryProcessor next)
      {
         _next = next;
      }

      public static string Version { get; set; }

      public static string RoleName { get; set; }

      public static string RoleInstance { get; set; }

      public void Process(ITelemetry item)
      {
         item.Context.Component.Version = Version;
         item.Context.Cloud.RoleInstance = RoleInstance;
         item.Context.Cloud.RoleName = RoleName;

         _next.Process(item);
      }
   }
}
