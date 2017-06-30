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
            .WriteTo.PoshConsole()
            .WriteTo.AzureApplicationInsights("b75b27a8-d3a2-4709-a2e3-5e99b07ba2ec");

         log.D("debug message");
         log.W("warning message");
         log.I("information");

         using (var dep = log.TrackDependency("coffeepot", "plug-in"))
         {
            Thread.Sleep(TimeSpan.FromSeconds(1));
         }

         using (var dep = log.TrackDependency("coffeepot", "plug-out"))
         {
            Thread.Sleep(TimeSpan.FromSeconds(1));

            dep.Add(new Exception("can't plug out, it's stuck!"));
         }

         for (int i = 0; i < 1000; i++)
         {
            log.D("event #{0} at {1}", i, DateTime.Now);
         }

         log.D("done, press a key");
         System.Console.WriteLine("done!");
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
