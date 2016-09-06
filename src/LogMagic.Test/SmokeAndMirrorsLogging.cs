using NUnit.Framework;
using System.IO;
using System;

namespace LogMagic.Test
{
   /// <summary>
   /// Only tests that logging on all the provider doesn't crash"
   /// </summary>
   [TestFixture("azure-blob")]
   [TestFixture("azure-table")]
   [TestFixture("files")]
   [TestFixture("console")]
   [TestFixture("posh-console")]
   [TestFixture("trace")]
   public class SmokeAndMirrorsLogging : AbstractTestFixture
   {
      private readonly string _receiverName;

      public SmokeAndMirrorsLogging(string receiverName)
      {
         _receiverName = receiverName;
      }

      [SetUp]
      public void SetUp()
      {
         L.Config.ClearWriters();
         var settings = new TestSettings();

         switch (_receiverName)
         {
            case "azure-blob":
               L.Config.WriteTo.AzureAppendBlob(settings.AzureStorageName, settings.AzureStorageKey,
                  "logs-integration", "smokeandmirrors");
               break;
            case "azure-table":
               L.Config.WriteTo.AzureTable(settings.AzureStorageName, settings.AzureStorageKey,
                  "logsintegration");
               break;
            case "files":
               L.Config.WriteTo.File(Path.Combine(TestDir.FullName, "subdir", "testlog.txt"));
               break;
            case "console":
               L.Config.WriteTo.Console();
               break;
            case "posh-console":
               L.Config.WriteTo.PoshConsole();
               break;
            case "trace":
               L.Config.WriteTo.Trace();
               break;
         }   
      }

      [TearDown]
      public void TearDown()
      {
         
      }

      [Test]
      public void Smoke_SomethingSimple_DoestCrash()
      {
         ILog log = L.G();
         ILog _log = L.G();

         log.D("hello!");
         log.D("exception is here!", new Exception("test exception"));

         L.Flush();
         //Thread.Sleep(TimeSpan.FromMinutes(1));
      }
   }
}
