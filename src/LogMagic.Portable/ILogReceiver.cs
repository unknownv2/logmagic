using System;

namespace LogMagic
{
   public interface ILogReceiver : IDisposable
   {
      void Send(LogChunk chunk);
   }
}
