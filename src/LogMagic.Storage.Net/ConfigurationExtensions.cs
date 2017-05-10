using LogMagic.Storage.Net;
using Storage.Net.Blob;
using Storage.Net.Table;
using System;

namespace LogMagic
{
   /// <summary>
   /// Extension methods to configure the integration
   /// </summary>
   public static class ConfigurationExtensions
   {
      /// <summary>
      /// Writes logs to a blob using <see cref="IBlobStorage.AppendFromStream(string, System.IO.Stream)"/> method
      /// </summary>
      /// <param name="configuration">Configuration reference</param>
      /// <param name="blobStorage">Valid blob storage reference</param>
      /// <param name="documentId">ID of the document to append to</param>
      /// <param name="format">Optional format string</param>
      /// <returns></returns>
      public static ILogConfiguration StorageAppendBlob(this IWriterConfiguration configuration,
         IBlobStorage blobStorage,
         string documentId,
         string format = null)
      {
         return configuration.Custom(new BlobStorageLogWriter(blobStorage, documentId, format));
      }

      public static ILogConfiguration StorageTables(this IWriterConfiguration configuration,
         ITableStorage tableStorage)
      {
         throw new NotImplementedException();
      }

      public static ILogConfiguration StorageMessagePublisher(this IWriterConfiguration configuration)
      {
         throw new NotImplementedException();
      }
   }
}