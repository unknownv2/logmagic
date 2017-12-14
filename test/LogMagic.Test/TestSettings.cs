using Config.Net;

namespace LogMagic.Test
{
   public interface ISettings
   {
      [Option(Alias = "Azure.Storage.Name")]
      string AzureStorageName { get; }

      [Option(Alias = "Azure.Storage.Key")]
      string AzureStorageKey { get; }

      [Option(Alias = "Azure.AppInsights.InstrumentationKey")]
      string AppInsightsKey { get; }
   }
}
