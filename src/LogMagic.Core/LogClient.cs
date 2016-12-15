using System;
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
         if(name == null) throw new ArgumentNullException(nameof(name));

         _name = name;
      }

      [MethodImpl(MethodImplOptions.NoInlining)]
      private void Serve(LogSeverity severity, string format, params object[] parameters)
      {
         LogEvent e = EventFactory.CreateEvent(_name, severity, format, parameters);
         LogEventPump.Queue(e);
      }

      [MethodImpl(MethodImplOptions.NoInlining)]
      public void D(string format, params object[] parameters)
      {
         Serve(LogSeverity.Debug, format, parameters);
      }

      [MethodImpl(MethodImplOptions.NoInlining)]
      public void E(string format, params object[] parameters)
      {
         Serve(LogSeverity.Error, format, parameters);
      }

      [MethodImpl(MethodImplOptions.NoInlining)]
      public void I(string format, params object[] parameters)
      {
         Serve(LogSeverity.Info, format, parameters);
      }

      [MethodImpl(MethodImplOptions.NoInlining)]
      public void W(string format, params object[] parameters)
      {
         Serve(LogSeverity.Warning, format, parameters);
      }
   }
}
