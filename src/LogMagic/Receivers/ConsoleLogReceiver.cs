using System;

namespace LogMagic.Receivers
{
   /// <summary>
   /// Adds log messages to system console
   /// </summary>
   public class ConsoleLogReceiver : ILogReceiver
   {
      public void Send(LogChunk chunk)
      {
         Console.WriteLine(@"{0}|{1}|{2}|{3}{4}",
            chunk.EventTime.ToString("H:mm:ss,fff"),
            chunk.SourceName,
            chunk.ThreadName,
            chunk.Message,
            chunk.Error == null ? string.Empty : (Environment.NewLine + chunk.Error));
      }

      public void Dispose()
      {
         //nothing to dispose in console
      }
   }
}
