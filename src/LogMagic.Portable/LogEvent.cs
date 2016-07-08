using System;
using System.Collections.Generic;

namespace LogMagic
{
   public class LogEvent
   {
      public const string ErrorPropertyName = "error";

      public LogEvent(LogSeverity severity, string sourceName, DateTime eventTime)
      {
         Severity = severity;
         SourceName = sourceName;
         EventTime = eventTime;
      }

      public string SourceName;

      public LogSeverity Severity;

      public DateTime EventTime;

      public string Message;

      public Dictionary<string, object> Properties;

      public void AddProperty(string name, object value)
      {
         if (name == null || value == null) return;

         if(Properties == null) Properties = new Dictionary<string, object>();

         Properties[name] = value;
      }

      public object GetProperty(string name)
      {
         if (Properties == null) return null;

         object r;
         if (!Properties.TryGetValue(name, out r)) return null;
         return r;
      }
   }
}
