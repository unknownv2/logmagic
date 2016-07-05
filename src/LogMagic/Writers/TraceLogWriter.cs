using System;
using System.Collections.Generic;
using System.Diagnostics;
using LogMagic.Formatters;
using LogMagic.Model;

namespace LogMagic.Writers
{
   /// <summary>
   /// Receives log chunks into standard <see cref="Trace"/>
   /// </summary>
   public class TraceLogWriter : ILogWriter
   {
      private ILogChunkFormatter _formatter;

      /// <summary>
      /// Creates an instance with standard formatter
      /// </summary>
      public TraceLogWriter() : this(null)
      {

      }

      /// <summary>
      /// Creates an instance with custom formatter
      /// </summary>
      public TraceLogWriter(ILogChunkFormatter formatter)
      {
         _formatter = formatter ?? new StandardFormatter();
      }

      /// <summary>
      /// Calls Trace.WriteLine on log chunk
      /// </summary>
      public void Send(LogChunk chunk)
      {
         Trace.WriteLine(_formatter.Format(chunk));
         Debug.WriteLine("test");
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

         }
      }
   }
}
