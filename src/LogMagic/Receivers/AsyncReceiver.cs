using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace LogMagic.Receivers
{
   /// <summary>
   /// Base class for async logger where writing procedure doesn't blobk the call and sends messages in
   /// a separate thread.
   /// </summary>
   public abstract class AsyncReceiver : ILogReceiver
   {
      private readonly ConcurrentQueue<LogChunk> _messageQueue = new ConcurrentQueue<LogChunk>();
      private readonly Thread _dispatchThread;
      private bool _disposed;

      protected AsyncReceiver()
      {
         _dispatchThread = new Thread(DispatchThreadEntry) {IsBackground = true, Priority = ThreadPriority.Lowest};
         _dispatchThread.Start();
      }

      public virtual void Send(LogChunk chunk)
      {
         _messageQueue.Enqueue(chunk);
      }

      public virtual void Dispose()
      {
         _disposed = true;
      }

      private void DispatchThreadEntry(object state)
      {
         var container = new List<LogChunk>(50);

         while (!_disposed)
         {
            container.Clear();

            LogChunk chunk;
            while(_messageQueue.TryDequeue(out chunk))
            {
               container.Add(chunk);

               if (container.Count == container.Capacity) break;
            }

            if (container.Count > 0)
            {
               SendChunks(container);
            }

            Thread.Sleep(TimeSpan.FromSeconds(1));
         }
      }

      protected abstract void SendChunks(IEnumerable<LogChunk> chunks);

   }
}
