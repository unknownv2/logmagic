using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using C = System.Console;

namespace LogMagic.Console
{
   public static class Program
   {
      private static readonly LogMagic.ILog log = LogMagic.L.G(typeof(Program));

      public static void Main(string[] args)
      {
         //TaskPumpTest();
         //System.Console.ReadLine(); return;

         //L.Config.WriteTo.Console();
         //L.Config.WriteTo.PoshConsole();
         //L.Config.WriteTo.File("c:\\tmp\\my.log");
         L.Config
            //.WriteTo.AzureApplicationInsights("6fd5b7d6-5844-478b-a9df-cfb49d5bd65e")
            .WriteTo.PoshConsole();

         log.D("debug message");
         log.W("warning message");
         log.I("information");

         for (int i = 0; i < 100; i++)
         {
            log.D("event at " + DateTime.UtcNow);
         }

         log.D("done, press a key");
         System.Console.ReadLine();
      }

      private static void TaskPumpTest()
      {
         var cts = new CancellationTokenSource();

         Task t = Task.Factory.StartNew(() => Step(cts.Token), cts.Token);

         C.WriteLine("c - cancel, number - send n event(s)");
         string input;
         while((input = C.ReadLine()) != "c")
         {
            evt.Set();
         }

         C.WriteLine("cancelling...");
         cts.Cancel();
         t.Wait();

         C.WriteLine("finished");
         C.ReadKey();
      }

      private static ManualResetEventSlim evt = new ManualResetEventSlim(false);

      private static void Step(CancellationToken token)
      {
         C.WriteLine("step");

         while(!token.IsCancellationRequested)
         {
            C.WriteLine("processing at " + DateTime.Now);

            try
            {
               evt.Wait(TimeSpan.FromSeconds(30), token);
               evt.Reset();
            }
            catch(OperationCanceledException)
            {

            }
         }

         C.WriteLine("step cancelled");
      }
   }
}
