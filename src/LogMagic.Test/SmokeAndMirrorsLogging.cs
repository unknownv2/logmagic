using Config.Net;
using Config.Net.Stores;
using LogMagic.WindowsAzure;
using NUnit.Framework;

namespace LogMagic.Test
{
   /// <summary>
   /// Only tests that logging on all the provider doesn't crash"
   /// </summary>
   [TestFixture("azure-blob")]
   public class SmokeAndMirrorsLogging
   {
      private static readonly Setting<string> AzureStorageName = new Setting<string>("Azure.Storage.Name", null);
      private static readonly Setting<string> AzureStorageKey = new Setting<string>("Azure.Storage.Key", null);

      private readonly string _receiverName;

      public SmokeAndMirrorsLogging(string receiverName)
      {
         _receiverName = receiverName;
      }

      [SetUp]
      public void SetUp()
      {
         L.ClearReceivers();
         Cfg.Configuration.RemoveAllStores();
         Cfg.Configuration.AddStore(new IniFileConfigStore("c:\\tmp\\integration-tests.ini"));

         switch (_receiverName)
         {
            case "azure-blob":
               L.AddReceiver(new AppendBlobLogReceiver(Cfg.Read(AzureStorageName), Cfg.Read(AzureStorageKey),
                  "logs-integration", "smokeandmirrors"));
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

         log.D("hello!");
      }
   }
}
