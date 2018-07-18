using LogMagic.Enrichers;
using LogMagic.Microsoft.Azure.ApplicationInsights;
using NetBox.Extensions;
using NetBox.Generator;
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
      private static readonly ILog log = L.G(typeof(Program));

      public static void Main(string[] args)
      {
         L.Config
            .WriteTo.Console()
            .WriteTo.PoshConsole()
            .WriteTo.Trace()
            .WriteTo.AzureApplicationInsights("13d9faf0-e96d-46ce-81b1-d8303c798765",
               new WriterOptions
               {
                  EnableQuickPulse = true,
                  FlushOnWrite = true,
                  CollectPerformanceCounters = true
               });
               //.When.SeverityIsAtLeast(LogSeverity.Information);

         while(true)
         {
            log.Trace("new event");

            Thread.Sleep(TimeSpan.FromSeconds(1));
         }

         /*log.Event("test event",
            KnownProperty.Severity, LogSeverity.Information);

         log.Critical("critical");

         using (L.Context(KnownProperty.OperationId, Guid.NewGuid().ToShortest()))
         {
            using (L.Context(
               KnownProperty.RoleInstance, Guid.NewGuid().ToString(),
               KnownProperty.RoleName, "service one"))
            {
               string requestId = Guid.NewGuid().ToString();
               string dependencyId = Guid.NewGuid().ToString();

               log.Request("incoming", 1, null, KnownProperty.ActivityId, requestId);
               log.Dependency("http", "server", "correlate", 1, null,
                  KnownProperty.ActivityId, dependencyId);

               log.Request("incoming@2", 1, null,
                  KnownProperty.RoleName, "service two",
                  KnownProperty.ParentActivityId, dependencyId);
            }
         }

         for(int i = 0; i < 10000; i++)
         {
            log.Event("event #" + i);

            Thread.Sleep(TimeSpan.FromMilliseconds(RandomGenerator.GetRandomInt(100, 5000)));
         }
         */

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

      public interface ILoggingInterface
      {
         void Succeed();

         void Fail();
      }

      public class LoggingImplementation : ILoggingInterface
      {
         public void Fail()
         {
            throw new ArgumentNullException("the failure signal");
         }

         public void Succeed()
         {
            
         }
      }
   }
}
