using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace LogMagic
{
   /// <summary>
   /// Global public logging configuration and initialisation class
   /// </summary>
   public static class L
   {
      private static readonly List<ILogReceiver> LogReceivers = new List<ILogReceiver>();
      private static readonly object EventLock = new object();

      /// <summary>
      /// Identifies this machine or node uniquely. Useful in situations when you are writing same or similar
      /// log lines into single location and need to know which node they are coming from.
      /// </summary>
      public static string NodeId { get; set; }

      /// <summary>
      /// Adds a receiver to log configuration
      /// </summary>
      /// <param name="receiver">Receiver instance</param>
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
            foreach (ILogReceiver receiver in LogReceivers)
            {
               receiver.Dispose();
            }

            LogReceivers.Clear();
         }
      }

      /// <summary>
      /// Get logger for the specified type
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <returns></returns>
      public static ILog G<T>()
      {
         return new LogClient(typeof(T), LogReceivers, EventLock);
      }

      /// <summary>
      /// Get logger for the specified type
      /// </summary>
      /// <param name="t"></param>
      /// <returns></returns>
      public static ILog G(Type t)
      {
         return new LogClient(t, LogReceivers, EventLock);
      }

#if !PORTABLE
      /// <summary>
      /// Get logger for the current class
      /// </summary>
      /// <returns></returns>
      public static ILog G()
      {
         return new LogClient(GetClassFullName(), LogReceivers, EventLock);
      }

      /// <summary>
      /// Gets the fully qualified name of the class invoking the LogManager, including the 
      /// namespace but not the assembly.    
      /// </summary>
      private static string GetClassFullName()
      {
         string className;
         Type declaringType;
         int framesToSkip = 2;

         do
         {
            StackFrame frame = new StackFrame(framesToSkip, false);
            MethodBase method = frame.GetMethod();
            declaringType = method.DeclaringType;
            if (declaringType == null)
            {
               className = method.Name;
               break;
            }

            framesToSkip++;
            className = declaringType.FullName;
         } while (declaringType.Module.Name.Equals("mscorlib.dll", StringComparison.OrdinalIgnoreCase));

         return className;
      }

#endif
   }
}
