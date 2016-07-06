using System;
using System.Collections.Generic;
using System.Diagnostics;

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
            string line = TextFormatter.Format(e);

            Trace.WriteLine(line);
            Debug.WriteLine(line);
         }
      }
   }
}
