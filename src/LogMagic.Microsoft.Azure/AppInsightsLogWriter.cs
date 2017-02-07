using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LogMagic.Microsoft.Azure
{
   class AppInsightsLogWriter : ILogWriter
   {
      private readonly TelemetryClient _tc = new TelemetryClient();

      public AppInsightsLogWriter(string instrumentationKey)
      {
         _tc.InstrumentationKey = instrumentationKey;
         _tc.Context.User.Id = Environment.UserName;
         _tc.Context.Session.Id = Guid.NewGuid().ToString();
         _tc.Context.Device.OperatingSystem = Environment.OSVersion.ToString();
      }

      public void Write(IEnumerable<LogEvent> events)
      {
         foreach(LogEvent e in events)
         {
            //_tc.TrackEvent(e.SourceName);
            _tc.TrackPageView(e.SourceName);
         }

         _tc.Flush();
      }

      public Task WriteAsync(IEnumerable<LogEvent> events)
      {
         Write(events);

         return Task.FromResult(true);
      }

      public void Dispose()
      {
         Thread.Sleep(TimeSpan.FromSeconds(5));

         _tc.Flush();
      }
   }
}
