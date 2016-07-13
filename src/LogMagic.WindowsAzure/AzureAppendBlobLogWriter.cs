using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace LogMagic.WindowsAzure
{
   /// <summary>
   /// Azure Append Blob Receiver
   /// </summary>
   class AzureAppendBlobLogWriter : ILogWriter
   {
      private readonly string _blobNamePrefix;
      //http://blogs.technet.com/b/thbrown/archive/2015/08/26/azure-blob-now-has-append.aspx

      private string _currentTagName;
      private readonly CloudBlobContainer _blobContainer;
      private CloudAppendBlob _appendBlob;

      /// <summary>
      /// Azure Append Blob Receiver
      /// </summary>
      /// <param name="storageAccountName">Storage account name</param>
      /// <param name="storageAccountKey">Storage account key (primary or secondary)</param>
      /// <param name="containerName">Blob container name</param>
      /// <param name="blobNamePrefix">
      /// Blob name prefix. Azure blobs will be named as 'blobNamePrefix-yyyy-MM-dd.txt'
      /// </param>
      public AzureAppendBlobLogWriter(string storageAccountName, string storageAccountKey, string containerName, string blobNamePrefix)
      {
         _blobNamePrefix = blobNamePrefix;
         var creds = new StorageCredentials(storageAccountName, storageAccountKey);
         var account = new CloudStorageAccount(creds, true);
         CloudBlobClient client = account.CreateCloudBlobClient();
         _blobContainer = client.GetContainerReference(containerName);
         _blobContainer.CreateIfNotExists();
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

      /// <summary>
      /// Sends chunks to append blob
      /// </summary>
      /// <param name="events"></param>
      public void Write(IEnumerable<LogEvent> events)
      {
         CloudAppendBlob blob = null;
         var sb = new StringBuilder();

         foreach (LogEvent e in events)
         {
            if (blob == null) blob = GetBlob(e.EventTime);

            string line = TextFormatter.Format(e, true);
            sb.AppendLine(line);
         }

         if (blob != null)
         {
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(sb.ToString())))
            {
               blob.AppendBlock(ms);
            }
         }

         //AppendText has a strange proble, whereas AppendBlock doesn't have it! The append position condition specified was not met.
         //blob?.AppendText(sb.ToString());
      }

      public void Dispose()
      {
      }
   }
}