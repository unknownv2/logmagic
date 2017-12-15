using LogMagic.Enrichers;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Fabric.Health;

namespace LogMagic.Microsoft.Azure.ServiceFabric.Writers
{
   class HealthReportWriter : ILogWriter
   {
      private readonly ServiceContext _context;

      public HealthReportWriter(ServiceContext context)
      {
         _context = context ?? throw new ArgumentNullException(nameof(context));
      }

      public void Write(IEnumerable<LogEvent> events)
      {
         foreach (LogEvent e in events)
         {
            object healthProperty = e.GetProperty(KnownProperty.ClusterHealthProperty);
            if (healthProperty == null) continue;


            var hi = new HealthInformation(e.SourceName, healthProperty.ToString(),
               e.ErrorException == null ? HealthState.Warning : HealthState.Error);

            _context.CodePackageActivationContext.ReportApplicationHealth(hi);
         }
      }

      public void Dispose()
      {
         throw new NotImplementedException();
      }

   }
}
