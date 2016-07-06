using LogMagic.WindowsAzure;

namespace LogMagic
{
   public static class ConfigurationExtensions
   {
      public static ILogConfiguration AzureAppendBlob(this IWriterConfiguration configuration,
         string storageAccountName,
         string storageAccountKey,
         string containerName,
         string blobNamePrefix)
      {
         return configuration.Custom(new AzureAppendBlobLogWriter(
            storageAccountName,
            storageAccountKey,
            containerName,
            blobNamePrefix));
      }

      public static ILogConfiguration AzureTable(this IWriterConfiguration configuration,
         string storageAccountName,
         string storageAccountKey,
         string tableName)
      {
         return configuration.Custom(new AzureTableLogWriter(storageAccountName, storageAccountKey, tableName));
      }
   }
}
