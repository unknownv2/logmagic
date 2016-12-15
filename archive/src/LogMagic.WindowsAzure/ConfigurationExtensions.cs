using LogMagic.WindowsAzure;

namespace LogMagic
{
   /// <summary>
   /// Helpers methods to configure logging library
   /// </summary>
   public static class ConfigurationExtensions
   {
      /// <summary>
      /// Writes to Azure Append Blob
      /// </summary>
      /// <param name="configuration">Configuration object</param>
      /// <param name="storageAccountName">Azure Storage account name</param>
      /// <param name="storageAccountKey">Azure Storage account key</param>
      /// <param name="containerName">Azure Storage container name</param>
      /// <param name="blobNamePrefix">Prefix to give to files in the blob storage i.e. "myapp-"</param>
      /// <returns></returns>
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

      /// <summary>
      /// Writes to Azure Table Storage
      /// </summary>
      /// <param name="configuration">Configuration object</param>
      /// <param name="storageAccountName">Azure Storage account name</param>
      /// <param name="storageAccountKey">Azure Storage account key</param>
      /// <param name="tableName">Name of the table to write to</param>
      /// <returns></returns>
      public static ILogConfiguration AzureTable(this IWriterConfiguration configuration,
         string storageAccountName,
         string storageAccountKey,
         string tableName)
      {
         return configuration.Custom(new AzureTableLogWriter(storageAccountName, storageAccountKey, tableName));
      }
   }
}
