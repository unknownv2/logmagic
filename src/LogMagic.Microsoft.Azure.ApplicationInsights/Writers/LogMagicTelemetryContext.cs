using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace LogMagic.Microsoft.Azure.ApplicationInsights.Writers
{
   class LogMagicTelemetryContext : ITelemetryProcessor
   {
      public string Version { get; set; }

      public string RoleName { get; set; }

      public string RoleInstance { get; set; }

      public void Process(ITelemetry item)
      {
         item.Context.Component.Version = Version;
         item.Context.Cloud.RoleInstance = RoleInstance;
         item.Context.Cloud.RoleName = RoleName;
      }
   }
}
