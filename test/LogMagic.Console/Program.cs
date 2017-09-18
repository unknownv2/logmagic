using LogMagic.Enrichers;
using System;
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
         L.Config
            .EnrichWith.Constant(KnownProperty.NodeName, "program.cs")
            .WriteTo.PoshConsole("{time:H:mm:ss,fff}|{level,-7}|{source}|{" + KnownProperty.NodeName + "}|{stack1}|{stack2}|{message}{error}")
            .WriteTo.AzureApplicationInsights("c9e98491-5d78-49f5-9439-bd32e460b44d", true);

         log.Trace("test");

         using (L.CP("stack1", "s11"))
         {
            log.Trace("test");

            using (L.CP("stack1", "s12"))
            {
               log.Trace("test");
            }
         }
         log.Trace("test");

         C.ReadLine();
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
