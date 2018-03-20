using LogMagic.Enrichers;
using NetBox.Extensions;
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
            .WriteTo.Trace()
            .WriteTo.AzureApplicationInsights("24703760-10ec-4e0b-b3ee-777f6ea80977", true);

         using (L.Operation())
         {
            using (L.Context(
               KnownProperty.RoleInstance, Guid.NewGuid().ToString(),
               KnownProperty.RoleName, "service one"))
            {
               string requestId = Guid.NewGuid().ToString();
               string dependencyId = Guid.NewGuid().ToString();

               log.Request("incoming", 1, null, KnownProperty.TelemetryId, requestId);
               log.Dependency("http", "server", "correlate", 1, null,
                  KnownProperty.TelemetryId, dependencyId);

               log.Request("incoming@2", 1, null,
                  KnownProperty.RoleName, "service two",
                  KnownProperty.OperationParentId, dependencyId);
            }
         }

         System.Console.ReadLine();
         return;

         using (L.Operation())
         {
            log.Trace("op: {0}, p: {1}", L.GetContextValue(KnownProperty.OperationId), L.GetContextValue(KnownProperty.OperationParentId));

            using (L.Operation())
            {
               log.Trace("op: {0}, p: {1}", L.GetContextValue(KnownProperty.OperationId), L.GetContextValue(KnownProperty.OperationParentId));
            }

            log.Trace("op: {0}, p: {1}", L.GetContextValue(KnownProperty.OperationId), L.GetContextValue(KnownProperty.OperationParentId));
         }

         log.Trace("op: {0}, p: {1}", L.GetContextValue(KnownProperty.OperationId), L.GetContextValue(KnownProperty.OperationParentId));

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
