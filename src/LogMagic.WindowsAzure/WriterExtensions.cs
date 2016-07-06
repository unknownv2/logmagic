using LogMagic.WindowsAzure;

namespace LogMagic
{
   public static class WriterExtensions
   {
      public static ILogConfiguration WriteToAzureAppendBlob(this ILogConfiguration configuration,
         string storageAccountName,
         string storageAccountKey,
         string containerName,
         string blobNamePrefix)
      {
         return configuration.AddWriter(new AzureAppendBlobLogWriter(
            storageAccountName,
            storageAccountKey,
            containerName,
            blobNamePrefix));
      }

      public static ILogConfiguration WriteToAzureTable(this ILogConfiguration configuration,
         string storageAccountName,
         string storageAccountKey,
         string tableName)
      {
         return configuration.AddWriter(new AzureTableLogWriter(storageAccountName, storageAccountKey, tableName));
      }
   }
}
