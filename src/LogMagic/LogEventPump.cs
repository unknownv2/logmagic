using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LogMagic
{
   static class LogEventPump
   {
      private const int BufferSize = 100;
      private static readonly TimeSpan ScanDelay = TimeSpan.FromMinutes(10);
      private static readonly ConcurrentQueue<LogEvent> _eventQueue = new ConcurrentQueue<LogEvent>();
      private static readonly Task logTask;
      private static ManualResetEventSlim logEvent = new ManualResetEventSlim(false);
      private static CancellationTokenSource cts = new CancellationTokenSource();

      static LogEventPump()
      {
         logTask = Task.Factory.StartNew(() => PumpMethod(cts.Token), cts.Token);
      }

      public static void Queue(LogEvent e)
      {
         _eventQueue.Enqueue(e);
         logEvent.Set();
      }

      public static void Shutdown()
      {
         cts.Cancel();
         logTask.Wait();
      }

      private static void PumpMethod(CancellationToken token)
      {
         while(!token.IsCancellationRequested)
         {
            while(!_eventQueue.IsEmpty)
            {
               var buffer = new List<LogEvent>();
               while(!_eventQueue.IsEmpty && buffer.Count < BufferSize)
               {
                  if (_eventQueue.TryDequeue(out LogEvent e)) buffer.Add(e);
               }

               if(buffer.Count > 0)
               {
                  Submit(buffer);
               }
            }

            try
            {
               logEvent.Wait(ScanDelay, token);
            }
            catch(OperationCanceledException)
            {

            }
         }
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
