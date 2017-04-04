using LogMagic.Storage.Net;
using Storage.Net.Blob;
using Storage.Net.Table;
using System;

namespace LogMagic
{
   public static class ConfigurationExtensions
   {
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