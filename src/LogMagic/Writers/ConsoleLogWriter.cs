using System;
using System.Collections.Generic;

namespace LogMagic.Writers
{
   /// <summary>
   /// Outputs messages to system console and ideal for server logging. It doesn't do anything fancy 
   /// unlike <see cref="PoshConsoleLogWriter"/>
   /// </summary>
   class ConsoleLogWriter : ILogWriter
   {
      /// <summary>
      /// Creates class instance
      /// </summary>
      public ConsoleLogWriter()
      {
         
      }

      /// <summary>
      /// There is nothing to dispose in the console
      /// </summary>
      public void Dispose()
      {
      }

      public void Write(IEnumerable<LogEvent> events)
      {
         foreach(LogEvent e in events)
         {
            Console.WriteLine(TextFormatter.Format(e, true));
         }
      }
   }
}
