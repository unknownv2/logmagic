using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogMagic.Console
{
   public class Program
   {
      private static readonly LogMagic.ILog log = LogMagic.L.G(typeof(Program));

      public static void Main(string[] args)
      {
         //L.Config.WriteTo.Console();
         //L.Config.WriteTo.PoshConsole();
         //L.Config.WriteTo.File("c:\\tmp\\my.log");
         L.Config
            .WriteTo.AzureApplicationInsights("6fd5b7d6-5844-478b-a9df-cfb49d5bd65e")
            .WriteTo.PoshConsole();

         log.D("debug message");
         log.W("warning message");
         log.I("information");

         for (int i = 0; i < 1000; i++)
         {
            log.D("event at " + DateTime.UtcNow);
         }

         System.Console.ReadLine();
      }
   }
}
