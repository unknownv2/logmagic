using System;

namespace LogMagic
{
   public interface ILogReceiver
   {
      void Send(LogSeverity severity,
         string sourceName,
         string threadName,
         DateTime eventTime,
         string message,
         Exception error);
   }
}
