using System;

namespace LogMagic
{
   /// <summary>
   /// Common interface for a log receiver
   /// </summary>
   public interface ILogReceiver : IDisposable
   {
      /// <summary>
      /// Sends a chunk of log data to the receiver
      /// </summary>
      void Send(LogChunk chunk);
   }
}
