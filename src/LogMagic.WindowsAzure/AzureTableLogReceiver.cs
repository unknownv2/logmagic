using System;
using System.Collections.Generic;
using LogMagic.Receivers;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;

namespace LogMagic.WindowsAzure
{
   public class AzureTableLogReceiver : AsyncReceiver
   {
      private readonly CloudTable _table;

      private class TableLogEntry : TableEntity
      {
         public string Severity { get; set; }

         public string SourceName { get; set; }

         public string ThreadName { get; set; }

         public string Message { get; set; }

         public string Error { get; set; }


         public static TableLogEntry FromLogChunk(LogChunk chunk)
         {
            var entry = new TableLogEntry();
            entry.PartitionKey = chunk.EventTime.ToString("yy-MM-dd");
            entry.RowKey = chunk.EventTime.ToString("HH-mm-ss-fff");
            entry.Severity = chunk.Severity.ToString();
            entry.SourceName = chunk.SourceName;
            entry.ThreadName = chunk.ThreadName;
            entry.Message = chunk.Message;
            if (chunk.Error != null) entry.Error = chunk.Error.ToString();

            return entry;
         }
      }

      public AzureTableLogReceiver(string storageAccountName, string storageAccountKey, string tableName)
      {
         var creds = new StorageCredentials(storageAccountName, storageAccountKey);
         var account = new CloudStorageAccount(creds, true);

         CloudTableClient tableClient = account.CreateCloudTableClient();
         _table = tableClient.GetTableReference(tableName);
         _table.CreateIfNotExists();
      }

      protected override void SendChunks(IEnumerable<LogChunk> chunks)
      {
         var batch = new TableBatchOperation();

         foreach (LogChunk chunk in chunks)
         {
            batch.Insert(TableLogEntry.FromLogChunk(chunk));
         }

         if (batch.Count > 0)
         {
            _table.ExecuteBatch(batch);
         }
      }
   }
}
