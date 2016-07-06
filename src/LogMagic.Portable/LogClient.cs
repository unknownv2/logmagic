using System;

namespace LogMagic
{
   /// <summary>
   /// Utility class to server log clients
   /// </summary>
   class LogClient : ILog
   {
      private readonly string _name;

      public LogClient(Type type) :
         this(type.Name)
      {
         
      }

      public LogClient(string name)
      {
         if(name == null) throw new ArgumentNullException(nameof(name));

         _name = name;
      }

      private void Serve(LogSeverity severity, string format, params object[] parameters)
      {
         LogEvent e = EventFactory.CreateEvent(_name, severity, format, parameters);
         LogEventPump.Queue(e);
      }

      public void D(string format, params object[] parameters)
      {
         Serve(LogSeverity.Debug, format, parameters);
      }

      public void E(string format, params object[] parameters)
      {
         Serve(LogSeverity.Error, format, parameters);
      }

      public void I(string format, params object[] parameters)
      {
         Serve(LogSeverity.Info, format, parameters);
      }

      public void W(string format, params object[] parameters)
      {
         Serve(LogSeverity.Warning, format, parameters);
      }
   }
}
