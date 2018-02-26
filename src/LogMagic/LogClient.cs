using LogMagic.Enrichers;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Diagnostics;
using NetBox.Extensions;
using System.Linq;

namespace LogMagic
{
   /// <summary>
   /// Utility class to server log clients
   /// </summary>
   class LogClient : ILog
   {
      private readonly string _name;

      public LogClient(Type type) :
         this(type.FullName)
      {
         
      }

      public LogClient(string name)
      {
         _name = name ?? throw new ArgumentNullException(nameof(name));
      }

      /// <summary>
      /// Logger name
      /// </summary>
      public string Name => _name;

      [MethodImpl(MethodImplOptions.NoInlining)]
      internal void Serve(
         EventType eventType,
         Dictionary<string, object> properties,
         string format,
         params object[] parameters)
      {
         LogEvent e = EventFactory.CreateEvent(_name, eventType, format, parameters);

         if(properties != null && properties.Count > 0)
         {
            foreach(KeyValuePair<string, object> prop in properties)
            {
               e.AddProperty(prop.Key, prop.Value);
            }
         }

         SubmitNow(e);
      }

      private void SubmitNow(LogEvent e)
      {
         foreach (ILogWriter writer in new List<ILogWriter>(L.Config.Writers))
         {
            try
            {
               IReadOnlyCollection<IFilter> filters = L.Config.GetFilters(writer);
               bool active = filters == null || filters.Any(f => f.Match(e));

               if (active)
               {
                  writer.Write(new[] { e });
               }
            }
            catch(Exception ex)
            {
               //there is nowhere else to log the error as we are the logger!
               Console.WriteLine("could not write: " + ex);

#if NETFULL
               System.Diagnostics.Trace.TraceError("fatal submit error: " + ex);
#endif
            }
         }
      }

      [MethodImpl(MethodImplOptions.NoInlining)]
      public void Trace(string format, params object[] parameters)
      {
         Serve(EventType.Trace, null, format, parameters);
      }

      [MethodImpl(MethodImplOptions.NoInlining)]
      public void Trace(string format, object[] parameters, params KeyValuePair<string, object>[] properties)
      {
         Serve(EventType.Trace, Create(properties), format, parameters);
      }

      [MethodImpl(MethodImplOptions.NoInlining)]
      public void Dependency(string type, string name, string command, long duration, Exception error, params KeyValuePair<string, object>[] properties)
      {
         Dictionary<string, object> ps = Create(properties);

         ps[KnownProperty.Duration] = duration;
         ps[KnownProperty.DependencyName] = name;
         ps[KnownProperty.DependencyType] = type;
         ps[KnownProperty.DependencyCommand] = command;

         var parameters = new List<object> { _name, command, TimeSpan.FromTicks(duration) };
         if (error != null) parameters.Add(error);

         Serve(EventType.Dependency, ps,
            "dependency {0}.{1} took {2}",
            parameters.ToArray());
      }

      [MethodImpl(MethodImplOptions.NoInlining)]
      public void Event(string name, params KeyValuePair<string, object>[] properties)
      {
         Dictionary<string, object> ps = Create(properties);
         ps[KnownProperty.EventName] = name;

         Serve(EventType.ApplicationEvent, ps,
            "application event {0} occurred",
            name);
      }

      [MethodImpl(MethodImplOptions.NoInlining)]
      public void Request(string name, long duration, Exception error, params KeyValuePair<string, object>[] properties)
      {
         Dictionary<string, object> ps = Create(properties);

         ps[KnownProperty.Duration] = duration;
         ps[KnownProperty.RequestName] = name;

         if (error != null)
         {
            ps[KnownProperty.Error] = error;
         }

         Serve(EventType.HandledRequest, ps,
            "request {0} took {1}", name, TimeSpan.FromTicks(duration));
      }

      [MethodImpl(MethodImplOptions.NoInlining)]
      public void Metric(string name, double value, params KeyValuePair<string, object>[] properties)
      {
         Dictionary<string, object> ps = Create(properties);

         ps[KnownProperty.MetricName] = name;
         ps[KnownProperty.MetricValue] = value;

         Serve(EventType.Metric, ps,
            "metric {0} == {1}",
            name, value);
      }

      private static Dictionary<string, object> Create(params KeyValuePair<string, object>[] properties)
      {
         var result = new Dictionary<string, object>();
         if (properties != null) result.AddRange(properties);
         return result;
      }
   }
}
