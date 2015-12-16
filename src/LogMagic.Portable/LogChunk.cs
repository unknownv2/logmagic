using System;

namespace LogMagic
{
   /// <summary>
   /// Contains log chunk information
   /// </summary>
   public struct LogChunk
   {
      /// <summary>
      /// Creates the chunk
      /// </summary>
      public LogChunk(LogSeverity severity, string sourceName, string threadName, DateTime eventTime, string message, Exception error)
      {
         Severity = severity;
         SourceName = sourceName;
         ThreadName = threadName;
         EventTime = eventTime;
         Message = message;
         Error = error;
      }

      /// <summary>
      /// Message severity
      /// </summary>
      public LogSeverity Severity;

      /// <summary>
      /// Source name. Usually it's the class name.
      /// </summary>
      public string SourceName;

      /// <summary>
      /// Thread name or ID
      /// </summary>
      public string ThreadName;

      /// <summary>
      /// Event time
      /// </summary>
      public DateTime EventTime;

      /// <summary>
      /// Message
      /// </summary>
      public string Message;

      /// <summary>
      /// When provided contains the roiginal error
      /// </summary>
      public Exception Error;
   }
}
