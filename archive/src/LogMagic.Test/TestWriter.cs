using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LogMagic.Test
{
   class TestWriter : ILogWriter
   {
      public string Message { get; private set; }
      public LogEvent Event { get; private set; }

      public void Dispose()
      {
      }

      public void Write(IEnumerable<LogEvent> events)
      {
         foreach (LogEvent e in events)
         {
            Message = e.FormattedMessage;
            Event = e;
         }
      }

      public Task WriteAsync(IEnumerable<LogEvent> events)
      {
         Write(events);
         return Task.FromResult(true);
      }
   }
}
