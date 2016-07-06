using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace LogMagic
{
   static class LogEventPump
   {
      private const int BufferSize = 100;
      private static readonly ConcurrentQueue<LogEvent> _eventQueue = new ConcurrentQueue<LogEvent>();
      private static ManualResetEvent _waitEvent = new ManualResetEvent(false);
      private static ManualResetEvent _shutdownCompleteEvent = new ManualResetEvent(false);
      private static bool _isRunning = true;

      static LogEventPump()
      {
         var t = new Thread(PumpMethod) { IsBackground = true };
         t.Start();
      }

      public static void Queue(LogEvent e)
      {
         _eventQueue.Enqueue(e);
         _waitEvent.Set();
      }

      public static void Shutdown()
      {
         _isRunning = false;
         _waitEvent.Set();

         _shutdownCompleteEvent.Reset();
         _shutdownCompleteEvent.WaitOne();
      }

      private static void PumpMethod()
      {
         while(_isRunning)
         {
            _waitEvent.Reset();

            while(!_eventQueue.IsEmpty)
            {
               var buffer = new List<LogEvent>();
               while(!_eventQueue.IsEmpty && buffer.Count < BufferSize)
               {
                  LogEvent e;
                  if (_eventQueue.TryDequeue(out e)) buffer.Add(e);
               }

               if(buffer.Count > 0)
               {
                  Submit(buffer);
               }
            }

            _waitEvent.WaitOne(TimeSpan.FromSeconds(5));
         }

         _shutdownCompleteEvent.Set();
      }

      internal static void Flush()
      {
         var buffer = new List<LogEvent>();
         LogEvent e;
         while (_eventQueue.TryDequeue(out e)) buffer.Add(e);
         Submit(buffer);
      }

      private static void Submit(List<LogEvent> events)
      {
         foreach(ILogWriter writer in new List<ILogWriter>(L.Config.Writers))
         {
            try
            {
               writer.Write(events);
            }
            catch(Exception ex)
            {
               //there is nowhere else to log the error as we are the logger!
               Console.WriteLine("could not write: " + ex);
            }
         }
      }
   }
}
