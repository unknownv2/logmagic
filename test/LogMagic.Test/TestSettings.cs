using Config.Net;

namespace LogMagic.Test
{
   class TestSettings : SettingsContainer
   {
      public Option<string> AzureStorageName = new Option<string>("Azure.Storage.Name", null);

      public Option<string> AzureStorageKey = new Option<string>("Azure.Storage.Key", null);

      protected override void OnConfigure(IConfigConfiguration configuration)
      {
         configuration.UseIniFile("c:\\tmp\\integration-tests.ini");
      }
   }
}
