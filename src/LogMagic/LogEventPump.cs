using System.Collections.Concurrent;

namespace LogMagic
{
   public static class LogEventPump
   {
      private static readonly ConcurrentQueue<LogEvent> _eventQueue = new ConcurrentQueue<LogEvent>();

      public static void Queue(LogEvent e)
      {
         _eventQueue.Enqueue(e);
      }
   }
}
