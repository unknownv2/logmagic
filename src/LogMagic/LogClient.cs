using LogMagic.Trackers;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

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

      [MethodImpl(MethodImplOptions.NoInlining)]
      internal void Serve(LogSeverity severity, EventType eventType,
         Dictionary<string, object> properties,
         string format,  params object[] parameters)
      {
         LogEvent e = EventFactory.CreateEvent(_name, severity, eventType, format, parameters);

         if(properties != null && properties.Count > 0)
         {
            foreach(var prop in properties)
            {
               e.AddProperty(prop.Key, prop.Value);
            }
         }

         LogEventPump.Queue(e);
      }

      [MethodImpl(MethodImplOptions.NoInlining)]
      public void D(string format, params object[] parameters)
      {
         Serve(LogSeverity.Debug, EventType.Trace, null, format, parameters);
      }

      [MethodImpl(MethodImplOptions.NoInlining)]
      public void E(string format, params object[] parameters)
      {
         Serve(LogSeverity.Error, EventType.Trace, null, format, parameters);
      }

      [MethodImpl(MethodImplOptions.NoInlining)]
      public void I(string format, params object[] parameters)
      {
         Serve(LogSeverity.Info, EventType.Trace, null, format, parameters);
      }

      [MethodImpl(MethodImplOptions.NoInlining)]
      public void W(string format, params object[] parameters)
      {
         Serve(LogSeverity.Warning, EventType.Trace, null, format, parameters);
      }

      [MethodImpl(MethodImplOptions.NoInlining)]
      public IDependencyTracker TrackDependency(string name, string command)
      {
         return new TimedDependencyTracker(name, command, this);
      }
   }
}
