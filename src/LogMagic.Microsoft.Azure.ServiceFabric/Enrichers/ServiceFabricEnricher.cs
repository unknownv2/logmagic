using LogMagic.Enrichers;
using System.Fabric;

namespace LogMagic.Microsoft.Azure.ServiceFabric.Enrichers
{
   class ServiceFabricEnricher : IEnricher
   {
      private ServiceContext _context;

      public ServiceFabricEnricher(ServiceContext context)
      {
         _context = context;
      }

      public void Enrich(LogEvent e, out string propertyName, out object propertyValue)
      {
         propertyName = null;
         propertyValue = null;

         e.AddProperty("applicationName", _context.CodePackageActivationContext.ApplicationName);
         e.AddProperty("applicationTypeName", _context.CodePackageActivationContext.ApplicationTypeName);
         e.AddProperty(KnownProperty.Version, _context.CodePackageActivationContext.CodePackageVersion);
         e.AddProperty(KnownProperty.NodeName, _context.NodeContext.NodeName);
         e.AddProperty("nodeType", _context.NodeContext.NodeType);
         e.AddProperty(KnownProperty.NodeIp, _context.NodeContext.IPAddressOrFQDN);
         e.AddProperty("partitionId", _context.PartitionId);
         e.AddProperty(KnownProperty.NodeInstanceId, _context.NodeContext.NodeInstanceId);
         e.AddProperty(KnownProperty.ApplicationName, _context.ServiceName);
         e.AddProperty("serviceTypeName", _context.ServiceTypeName);
      }
   }
}
