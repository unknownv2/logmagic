using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace LogMagic.Writers
{
   /// <summary>
   /// Receives log chunks into standard <see cref="Trace"/>
   /// </summary>
   class TraceLogWriter : ILogWriter
   {
      /// <summary>
      /// Creates an instance with standard formatter
      /// </summary>
      public TraceLogWriter()
      {

      }

      /// <summary>
      /// Nothing to dispose
      /// </summary>
      public void Dispose()
      {
      }

      public void Write(IEnumerable<LogEvent> events)
      {
         foreach(LogEvent e in events)
         {
            string line = TextFormatter.Format(e, true);

            switch(e.Severity)
            {
               case LogSeverity.Debug:
                  Trace.WriteLine(line);
                  break;
               case LogSeverity.Error:
                  Trace.TraceError(line);
                  break;
               case LogSeverity.Info:
                  Trace.TraceInformation(line);
                  break;
               case LogSeverity.Warning:
                  Trace.TraceWarning(line);
                  break;
            }
         }
      }

      public Task WriteAsync(IEnumerable<LogEvent> events)
      {
         Write(events);

         return Task.FromResult(true);
      }
   }
}
