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

         L.Shutdown();
      }

      private void Run()
      {
         _log.I("hello, LogMagic!");

         _log.W("we are going to divide by zero!");

         int a = 10, b = 0;

         try
         {
            _log.D("dividing {a} by {b}", a, b);
            Console.WriteLine(a / b);
         }
         catch(Exception ex)
         {
            _log.E("unexpected error", ex);
         }

         _log.D("attempting to divide by zero");
      }

   }
}
