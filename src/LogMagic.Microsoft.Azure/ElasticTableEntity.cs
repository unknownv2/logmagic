using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace LogMagic.Microsoft.Azure
{
   class ElasticTableEntity : ITableEntity
   {
      private Dictionary<string, EntityProperty> _properties;

      public string ETag { get; set; }

      public string PartitionKey { get; set; }

      public string RowKey { get; set; }

      public DateTimeOffset Timestamp { get; set; }

      public void ReadEntity(IDictionary<string, EntityProperty> properties, OperationContext operationContext)
      {
         _properties = properties == null
            ? new Dictionary<string, EntityProperty>()
            : new Dictionary<string, EntityProperty>(properties);
      }

      public IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext)
      {
         return _properties;
      }

      public void Add(string name, object value)
      {
         if (string.IsNullOrEmpty(name) || value == null) return;

         if (_properties == null) _properties = new Dictionary<string, EntityProperty>();

         _properties.Add(name, new EntityProperty(value.ToString()));
      }
   }
}
