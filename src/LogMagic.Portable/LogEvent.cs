using System;
using System.Collections.Generic;

namespace LogMagic
{
   public class LogEvent
   {
      public LogEvent(LogSeverity severity, string sourceName, DateTime eventTime, string message, Exception error)
      {

      }

      public string SourceName;

      public LogSeverity Severity;

      public DateTime EventTime;

      public string Message;

      public Exception Error;

      public Dictionary<string, string> Properties;
   }
}
