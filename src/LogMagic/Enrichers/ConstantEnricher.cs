using System.Collections.Generic;

namespace LogMagic.Enrichers
{
   class ConstantEnricher : IEnricher
   {
      private readonly string _propertyName;
      private readonly string _propertyValue;

      public ConstantEnricher(string propertyName, string propertyValue)
      {
         _propertyName = propertyName;
         _propertyValue = propertyValue;
      }

      public ConstantEnricher(KeyValuePair<string, string> constant) : this(constant.Key, constant.Value)
      {

      }

      public void Enrich(LogEvent e, out string propertyName, out object propertyValue)
      {
         propertyName = _propertyName;
         propertyValue = _propertyValue;
      }
   }
}
