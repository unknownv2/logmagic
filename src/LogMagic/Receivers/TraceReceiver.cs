using System;
using System.Diagnostics;
using LogMagic.Formatters;

namespace LogMagic.Receivers
{
   /// <summary>
   /// Receives log chunks into standard <see cref="Trace"/>
   /// </summary>
   public class TraceReceiver : ILogReceiver
   {
      private ILogChunkFormatter _formatter;

      /// <summary>
      /// Creates an instance with standard formatter
      /// </summary>
      public TraceReceiver() : this(null)
      {

      }

      /// <summary>
      /// Creates an instance with custom formatter
      /// </summary>
      public TraceReceiver(ILogChunkFormatter formatter)
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
   }
}
