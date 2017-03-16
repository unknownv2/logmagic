using Config.Net;

namespace LogMagic.Test
{
   class TestSettings : SettingsContainer
   {
      public readonly Option<string> AzureStorageName = new Option<string>("Azure.Storage.Name", null);

      public readonly Option<string> AzureStorageKey = new Option<string>("Azure.Storage.Key", null);

      public readonly Option<string> AppInsightsKey = new Option<string>("Azure.AppInsights.InstrumentationKey", null);

      protected override void OnConfigure(IConfigConfiguration configuration)
      {
         configuration.UseIniFile("c:\\tmp\\integration-tests.ini");
      }
   }
}
