using System;
using System.Collections.Generic;

namespace LogMagic.Seq
{
   class RawEvent
   {
      public DateTimeOffset Timestamp { get; set; }

      // Uses the Serilog level names
      public string Level { get; set; }

      public string MessageTemplate { get; set; }

      public Dictionary<string, object> Properties { get; set; }

      public string Exception { get; set; }

      public static RawEvent FromLogEvent(LogEvent e)
      {
         return new RawEvent
         {
            Timestamp = new DateTimeOffset(e.EventTime),
            Level = e.Severity.ToString(),
            MessageTemplate = e.Message,
            Properties = e.Properties,
            Exception = (e.GetProperty(LogEvent.ErrorPropertyName) as Exception)?.ToString(),
         };
      }
   }
}
