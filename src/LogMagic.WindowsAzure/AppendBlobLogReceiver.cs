using System;
using System.Collections.Generic;
using LogMagic.Receivers;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace LogMagic.WindowsAzure
{
   public class AppendBlobLogReceiver : AsyncReceiver
   {
      //http://blogs.technet.com/b/thbrown/archive/2015/08/26/azure-blob-now-has-append.aspx

      public AppendBlobLogReceiver(string storageAccountName, string storageAccountKey, string containerName, string blobNamePrefix)
      {
         var creds = new StorageCredentials(storageAccountName, storageAccountKey);
         var account = new CloudStorageAccount(creds, true);
         CloudBlobClient client = account.CreateCloudBlobClient();
         var container = client.GetContainerReference(containerName);
         container.CreateIfNotExists();
      }

      protected override void SendChunks(IEnumerable<LogChunk> chunks)
      {
         foreach (LogChunk chunk in chunks)
         {
            string errorString = chunk.Error == null ? string.Empty : (Environment.NewLine + chunk.Error);
            string line = $"{chunk.EventTime.ToString("H:mm:ss,fff")}|{chunk.SourceName}|{chunk.ThreadName}|{chunk.Message}{errorString}";
         }
      }
   }
}