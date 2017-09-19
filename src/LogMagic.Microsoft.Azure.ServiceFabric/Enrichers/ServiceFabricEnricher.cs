using LogMagic.Enrichers;
using System.Fabric;

namespace LogMagic.Microsoft.Azure.ServiceFabric.Enrichers
{
   class ServiceFabricEnricher : IEnricher
   {
      private readonly ServiceContext _context;

      public ServiceFabricEnricher(ServiceContext context)
      {
         _context = context ?? throw new System.ArgumentNullException(nameof(context));
      }

      public void Enrich(LogEvent e, out string propertyName, out object propertyValue)
      {
         propertyName = null;
         propertyValue = null;

         e.AddProperty(KnownFabricProperty.ServiceName, _context.ServiceName);
         e.AddProperty(KnownFabricProperty.ServiceTypeName, _context.ServiceTypeName);
         e.AddProperty(KnownFabricProperty.PartitionId, _context.PartitionId);
         e.AddProperty(KnownFabricProperty.ApplicationName, _context.CodePackageActivationContext.ApplicationName);
         e.AddProperty(KnownFabricProperty.ApplicationTypeName, _context.CodePackageActivationContext.ApplicationTypeName);
         e.AddProperty(KnownFabricProperty.NodeName, _context.NodeContext.NodeName);
         e.AddProperty(KnownProperty.Version, _context.CodePackageActivationContext.CodePackageVersion);

         if (_context is StatelessServiceContext)
         {
            e.AddProperty(KnownFabricProperty.InstanceId, _context.ReplicaOrInstanceId);
         }

         if(_context is StatefulServiceContext)
         {
            e.AddProperty(KnownFabricProperty.ReplicaId, _context.ReplicaOrInstanceId);
         }
      }
   }
}
