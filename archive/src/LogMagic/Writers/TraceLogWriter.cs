using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using LogMagic.Tokenisation;

namespace LogMagic.Writers
{
   /// <summary>
   /// Receives log chunks into standard <see cref="Trace"/>
   /// </summary>
   class TraceLogWriter : ILogWriter
   {
      private readonly FormattedString _format;

      /// <summary>
      /// Creates an instance with standard formatter
      /// </summary>
      public TraceLogWriter(string format)
      {
         _format = format == null ? null : FormattedString.Parse(format, null);
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
            string line = TextFormatter.Format(e, _format);

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
