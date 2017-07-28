using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace LogMagic
{
   static class LogEventPump
   {
      private const int BufferSize = 100;
      private static readonly TimeSpan ScanDelay = TimeSpan.FromMinutes(1);
      private static readonly ConcurrentQueue<LogEvent> _eventQueue = new ConcurrentQueue<LogEvent>();
      private static readonly Task logTask;
      private static ManualResetEventSlim logEvent = new ManualResetEventSlim(false);
      private static CancellationTokenSource cts = new CancellationTokenSource();

      static LogEventPump()
      {
         Task.Factory.StartNew(SubmitLoopAsync, TaskCreationOptions.LongRunning);
      }

      public static void Queue(LogEvent e)
      {
         _eventQueue.Enqueue(e);
      }

      private static async Task SubmitLoopAsync()
      {
         CancellationToken token = cts.Token;

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
                  await SubmitAsync(buffer);
               }
            }

            try
            {
               logEvent.Wait(ScanDelay, token);
            }
            catch(OperationCanceledException)
            {

            }
            catch(Exception ex)
            {
#if NETFULL
               Trace.TraceError($"unhandled exception while waiting for more events: {ex}");
#endif
               cts.Cancel();
            }
         }
      }

      private static async Task SubmitAsync(List<LogEvent> events)
      {
         foreach (ILogWriter writer in new List<ILogWriter>(L.Config.Writers))
         {
            try
            {
               Task t = writer.WriteAsync(events);
            }
            catch (Exception ex)
            {
               //there is nowhere else to log the error as we are the logger!
               Console.WriteLine("could not write: " + ex);

#if NETFULL
               Trace.TraceError("fatal submit error: " + ex);
#endif
            }
         }
      }

      public static void Shutdown()
      {
         cts.Cancel();
      }

   }
}
