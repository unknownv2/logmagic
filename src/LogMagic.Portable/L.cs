using System;
using System.Collections.Generic;

namespace LogMagic
{
   public static class L
   {
      private static readonly List<ILogReceiver> LogReceivers = new List<ILogReceiver>();
      private static readonly object EventLock = new object();

      public static void AddReceiver(ILogReceiver receiver)
      {
         if(receiver == null) throw new ArgumentNullException(nameof(receiver));

         lock(LogReceivers)
         {
            LogReceivers.Add(receiver);
         }
      }

      /// <summary>
      /// Removes all log receivers
      /// </summary>
      public static void ClearReceivers()
      {
         lock (LogReceivers)
         {
            LogReceivers.Clear();
         }
      }

      public static ILog G<T>()
      {
         return new LogClient(typeof(T), LogReceivers, EventLock);
      }

      public static ILog G(Type t)
      {
         return new LogClient(t, LogReceivers, EventLock);
      }
   }
}
