using System;
using System.Collections.Generic;
using LogMagic.Formatters;
using LogMagic.Receivers;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace LogMagic.WindowsAzure
{
   public class AzureAppendBlobLogReceiver : AsyncReceiver
   {
      private readonly string _blobNamePrefix;
      //http://blogs.technet.com/b/thbrown/archive/2015/08/26/azure-blob-now-has-append.aspx

      private string _currentTagName;
      private readonly CloudBlobContainer _blobContainer;
      private CloudAppendBlob _appendBlob;
      private readonly IFormatter _formatter;

      public AzureAppendBlobLogReceiver(string storageAccountName, string storageAccountKey, string containerName,
         string blobNamePrefix)
         : this(storageAccountName, storageAccountKey, containerName, blobNamePrefix, null)
      {
         
      }

      public AzureAppendBlobLogReceiver(string storageAccountName, string storageAccountKey, string containerName, string blobNamePrefix, IFormatter formatter)
      {
         _blobNamePrefix = blobNamePrefix;
         var creds = new StorageCredentials(storageAccountName, storageAccountKey);
         var account = new CloudStorageAccount(creds, true);
         CloudBlobClient client = account.CreateCloudBlobClient();
         _blobContainer = client.GetContainerReference(containerName);
         _blobContainer.CreateIfNotExists();
         _formatter = formatter ?? new StandardFormatter();
      }

      private CloudAppendBlob GetBlob(DateTime eventTime)
      {
         string tagName = eventTime.ToString("yyyy-MM-dd");

         if (tagName != _currentTagName)
         {
            _currentTagName = tagName;
            _appendBlob = _blobContainer.GetAppendBlobReference($"{_blobNamePrefix}{_currentTagName}.txt");
            if(!_appendBlob.Exists()) _appendBlob.CreateOrReplace();
         }

         return _appendBlob;
      }

      protected override void SendChunks(IEnumerable<LogChunk> chunks)
      {
         foreach (LogChunk chunk in chunks)
         {
            string line = _formatter.Format(chunk);

            GetBlob(chunk.EventTime).AppendText(line);
         }
      }
   }
}