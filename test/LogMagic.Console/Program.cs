using LogMagic.Enrichers;
using System;
using System.Threading;
using System.Threading.Tasks;
using C = System.Console;

namespace LogMagic.Console
{
   public static class Program
   {
      private static readonly LogMagic.ILog log = L.G(typeof(Program));

      public static void Main(string[] args)
      {
         L.Config
            .EnrichWith.Constant(KnownProperty.NodeName, "program.cs")
            .EnrichWith.Constant(KnownProperty.OperationId, Guid.NewGuid().ToString())
            .WriteTo.PoshConsole("{time:H:mm:ss,fff}|{level,-7}|{source}|{" + KnownProperty.NodeName + "}|{stack1}|{stack2}|{message}{error}")
            .WriteTo.AzureApplicationInsights("24703760-10ec-4e0b-b3ee-777f6ea80977", true);

         log.Trace("test");

         using (L.Context("stack1".PairedWith("s11")))
         {
            log.Trace("test");

            using (L.Context("stack1".PairedWith("s12")))
            {
               log.Trace("s1 - {0}", L.GetContextValue("stack1"));

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
