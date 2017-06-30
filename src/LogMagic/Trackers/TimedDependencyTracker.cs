using LogMagic.Enrichers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace LogMagic.Trackers
{
   class TimedDependencyTracker : IDependencyTracker
   {
      private readonly Stopwatch _stopwatch = new Stopwatch();
      private readonly string _name;
      private readonly string _command;
      private readonly LogClient _client;
      private Exception _error;

      public TimedDependencyTracker(string name, string command, LogClient client)
      {
         _name = name;
         _command = command;
         _client = client;
         _stopwatch.Start();
      }

      public void Add(Exception e)
      {
         _error = e;
      }

      public void Dispose()
      {
         long ticks = _stopwatch.ElapsedTicks;
         _stopwatch.Stop();

         var properties = new Dictionary<string, object>
         {
            { KnownProperty.Duration, ticks },
            { KnownProperty.ApplicationName, _name },
            { KnownProperty.MethodName, _command }
         };

         var parameters = new List<object> { _name, _command, TimeSpan.FromTicks(ticks) };
         if (_error != null) parameters.Add(_error);

         _client.Serve(LogSeverity.Info, EventType.Dependency, properties,
            "dependency {0}.{1} took {2}",
            parameters.ToArray());
      }
   }
}
