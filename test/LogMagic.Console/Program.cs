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
         L.Config.WriteTo.File("c:\\tmp\\my.log");

         log.D("debug message");
         log.W("warning message");
         log.I("information");

         System.Console.ReadLine();
      }
   }
}
