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
         var re = new RawEvent
         {
            Timestamp = new DateTimeOffset(e.EventTime),
            Level = e.Severity.ToString(),
            MessageTemplate = e.Message,
            Exception = (e.GetProperty(LogEvent.ErrorPropertyName) as Exception)?.ToString(),
         };

         re.Properties = new Dictionary<string, object>(e.Properties);
         re.Properties.Add("source", e.SourceName);

         return re;
      }
   }
}
