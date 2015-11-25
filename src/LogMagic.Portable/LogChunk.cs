using System;

namespace LogMagic
{
   public struct LogChunk
   {
      public LogChunk(LogSeverity severity, string sourceName, string threadName, DateTime eventTime, string message, Exception error)
      {
         Severity = severity;
         SourceName = sourceName;
         ThreadName = threadName;
         EventTime = eventTime;
         Message = message;
         Error = error;
      }

      public LogSeverity Severity;

      public string SourceName;

      public string ThreadName;

      public DateTime EventTime;

      public string Message;

      public Exception Error;
   }
}
