using LogMagic.Enrichers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace LogMagic.Trackers
{
   class TimedRequestTracker : IRequestTracker
   {
      private readonly Stopwatch _stopwatch = new Stopwatch();
      private readonly string _name;
      private readonly LogClient _client;
      private Exception _error;

      public TimedRequestTracker(string name, LogClient client)
      {
         _name = name;
         _client = client;
         _stopwatch.Start();
      }

      public void Add(Exception e)
      {
         _error = e;
      }

      public void Dispose()
      {
         _stopwatch.Stop();
         long ticks = _stopwatch.Elapsed.Ticks;

         var properties = new Dictionary<string, object>
         {
            { KnownProperty.Duration, ticks },
            { KnownProperty.ApplicationName, _name }
         };

         var parameters = new List<object> { _name, TimeSpan.FromTicks(ticks) };
         if (_error != null) parameters.Add(_error);

         _client.Serve(LogSeverity.Info, EventType.HandledRequest, properties,
            "request {0} took {1}",
            parameters.ToArray());
      }
   }
}
