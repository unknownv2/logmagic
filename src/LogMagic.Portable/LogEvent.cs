using System;
using System.Collections.Generic;

namespace LogMagic
{
   /// <summary>
   /// Represents a log event
   /// </summary>
   public class LogEvent
   {
      /// <summary>
      /// Standard property name to store error information
      /// </summary>
      public const string ErrorPropertyName = "error";

      /// <summary>
      /// Constructs a new instance of a log event
      /// </summary>
      public LogEvent(LogSeverity severity, string sourceName, DateTime eventTime)
      {
         Severity = severity;
         SourceName = sourceName;
         EventTime = eventTime;
      }

      /// <summary>
      /// Name of the source, usually class name or type
      /// </summary>
      public string SourceName;

      /// <summary>
      /// Log severity
      /// </summary>
      public LogSeverity Severity;

      /// <summary>
      /// Time in UTC when log event has occurred
      /// </summary>
      public DateTime EventTime;

      /// <summary>
      /// Formatted log message
      /// </summary>
      public string Message;

      /// <summary>
      /// Extra properties
      /// </summary>
      public Dictionary<string, object> Properties;

      /// <summary>
      /// Tries to find the error and cast to <see cref="System.Exception"/> class, otherwise returns null
      /// </summary>
      public Exception ErrorException
      {
         get
         {
            object exceptionObject = GetProperty(ErrorPropertyName);
            return exceptionObject as Exception;
            
         }
      }

      /// <summary>
      /// Adds a new property to this event
      /// </summary>
      public void AddProperty(string name, object value)
      {
         if (name == null || value == null) return;

         if(Properties == null) Properties = new Dictionary<string, object>();

         Properties[name] = value;
      }

      /// <summary>
      /// Tries to get the log property by name
      /// </summary>
      public object GetProperty(string name)
      {
         if (Properties == null) return null;

         object r;
         if (!Properties.TryGetValue(name, out r)) return null;
         return r;
      }
   }
}
