using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;

namespace LogMagic.WindowsAzure
{
   /// <summary>
   /// Azure Table Storage receiver
   /// </summary>
   class AzureTableLogWriter : ILogWriter
   {
      private readonly CloudTable _table;

      private class TableLogEntry : TableEntity
      {
         public string Severity { get; set; }

         public string SourceName { get; set; }

         public string ThreadName { get; set; }

         public string Message { get; set; }

         public string Error { get; set; }


         public static TableLogEntry FromLogEvent(LogEvent e)
         {
            var entry = new TableLogEntry();
            entry.PartitionKey = e.EventTime.ToString("yy-MM-dd");
            entry.RowKey = e.EventTime.ToString("HH-mm-ss-fff");
            entry.Severity = e.Severity.ToString();
            entry.SourceName = e.SourceName;
            entry.Message = e.Message;
            entry.Error = (e.GetProperty(LogEvent.ErrorPropertyName) as Exception)?.ToString();

            return entry;
         }
      }

      /// <summary>
      /// Creates class instance
      /// </summary>
      /// <param name="storageAccountName">Storage account name</param>
      /// <param name="storageAccountKey">Storage account key</param>
      /// <param name="tableName">Target table name</param>
      public AzureTableLogWriter(string storageAccountName, string storageAccountKey, string tableName)
      {
         var creds = new StorageCredentials(storageAccountName, storageAccountKey);
         var account = new CloudStorageAccount(creds, true);

         CloudTableClient tableClient = account.CreateCloudTableClient();
         _table = tableClient.GetTableReference(tableName);
         _table.CreateIfNotExists();
      }

      /// <summary>
      /// Sends chunks to table
      /// </summary>
      /// <param name="events"></param>
      public void Write(IEnumerable<LogEvent> events)
      {
         var batch = new TableBatchOperation();

         foreach (LogEvent e in events)
         {
            batch.Insert(TableLogEntry.FromLogEvent(e));
         }

         if (batch.Count > 0)
         {
            _table.ExecuteBatch(batch);
         }
      }

      public void Dispose()
      {
      }
   }
}
