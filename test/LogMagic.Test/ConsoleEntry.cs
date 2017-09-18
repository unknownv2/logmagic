using System;
using LogMagic;

namespace LogMagicExample
{
   public class Program
   {
      private readonly ILog _log = L.G();

      public static void Main(string[] args)
      {
         L.Config
            .WriteTo.PoshConsole("{time:H:mm:ss,fff}|{level}|{threadId,4}|{source}|{message}{error}")
            .WriteTo.Console()
            .EnrichWith.ThreadId();

         new Program().Run();

         Console.ReadLine();
      }

      private void Run()
      {
         _log.Trace("hello, LogMagic!");

         _log.Trace("we are going to divide by zero!");

         int a = 10, b = 0;

         try
         {
            _log.Trace("dividing {a} by {b}", a, b);
            Console.WriteLine(a / b);
         }
         catch(Exception ex)
         {
            _log.Trace("unexpected error", ex);
         }

         _log.Trace("attempting to divide by zero");
      }

   }
}
