using System;
using System.Collections.Concurrent;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace LogMagic.WindowsAzure
{
   public class AppendBlobLogReceiver : ILogReceiver
   {
      //http://blogs.technet.com/b/thbrown/archive/2015/08/26/azure-blob-now-has-append.aspx

      private readonly CloudBlobContainer _container;
      private readonly ConcurrentQueue<string> _messageQueue = new ConcurrentQueue<string>();

      public AppendBlobLogReceiver(string storageAccountName, string storageAccountKey, string containerName, string blobNamePrefix)
      {
         var creds = new StorageCredentials(storageAccountName, storageAccountKey);
         var account = new CloudStorageAccount(creds, true);
         CloudBlobClient client = account.CreateCloudBlobClient();
         _container = client.GetContainerReference(containerName);
         _container.CreateIfNotExists();
      }

      public void Send(LogSeverity severity, string sourceName, string threadName, DateTime eventTime, string message,
         Exception error)
      {


         throw new NotImplementedException();
      }
   }
}