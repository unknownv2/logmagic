using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using LogMagic.TypeFormatters;

namespace LogMagic
{
   /// <summary>
   /// Utility class to server log clients
   /// </summary>
   class LogClient : ILog
   {
      private readonly IEnumerable<ILogWriter> _receivers;
      private readonly object _eventLock;
      private readonly string _name;

      public LogClient(Type type, IEnumerable<ILogWriter> receivers, object eventLock) :
         this(type.Name, receivers, eventLock)
      {
         
      }

      public LogClient(string name, IEnumerable<ILogWriter> receivers, object eventLock)
      {
         if(name == null) throw new ArgumentNullException(nameof(name));
         if(receivers == null) throw new ArgumentNullException(nameof(receivers));
         if(eventLock == null) throw new ArgumentNullException(nameof(eventLock));

         _name = name;
         _receivers = receivers;
         _eventLock = eventLock;
      }

      private void Serve(LogSeverity severity, string format, params object[] parameters)
      {

#if PORTABLE
         string threadName = Task.CurrentId.ToString();
#else
         string threadName = Thread.CurrentThread.ManagedThreadId.ToString();
#endif

         DateTime eventTime = DateTime.UtcNow;
         Exception error;

         if(parameters != null && parameters.Length > 0)
         {
            error = parameters[parameters.Length - 1] as Exception;
            if(error != null)
            {
               Array.Resize(ref parameters, parameters.Length - 1);
            }
         }
         else
         {
            error = null;
         }

         string message = parameters == null ? format : string.Format(format, parameters.Select(FormatterEntry.FormatParameter).ToArray());
         PushToReceivers(severity, threadName, eventTime, message, error);
      }

      private void PushToReceivers(
         LogSeverity severity,
         string threadName,
         DateTime eventTime,
         string message,
         Exception error)
      {
         //send the message
         lock (_eventLock)
         {
            foreach (ILogWriter receiver in _receivers)
            {
               receiver.Write(new List<LogEvent> { new LogEvent(severity, _name, eventTime, message, error)});
            }
         }
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
