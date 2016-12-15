using System;

namespace LogMagic.Enrichers
{
   class MachineNameEnricher : IEnricher
   {
      private readonly string _machineName;

      public MachineNameEnricher()
      {
         _machineName = Environment.MachineName;
      }

      public void Enrich(LogEvent e, out string propertyName, out object propertyValue)
      {
         propertyName = "machineName";
         propertyValue = _machineName;
      }
   }
}
