using System;
using System.Diagnostics;

namespace LogMagic
{
   static class LogEventPump
   {
      public static void Queue(LogEvent e)
      {
         foreach (ILogWriter writer in L.Config.Writers)
         {
            try
            {
               //todo: use asynchronouse versions
               writer.Write(new[] { e });
            }
            catch(Exception ex)
            {
               //swallow this
               Debug.WriteLine("failed to log, " + ex);
            }
         }
      }
   }
}
