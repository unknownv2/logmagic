using System;
using LogMagic.Formatters;

namespace LogMagic.Receivers
{
   /// <summary>
   /// Adds log messages to system console
   /// </summary>
   public class ConsoleLogReceiver : ILogReceiver
   {
      private readonly IFormatter _formatter;

      public ConsoleLogReceiver() : this(null)
      {
         
      }

      public ConsoleLogReceiver(IFormatter formatter)
      {
         _formatter = formatter ?? new StandardFormatter();
      }

      public void Send(LogChunk chunk)
      {
         Console.Write(_formatter.Format(chunk));
      }

      public void Dispose()
      {
         //nothing to dispose in console
      }
   }
}
