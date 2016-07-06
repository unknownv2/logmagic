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

      public void Enrich(LogEvent e, out string propertyName, out string propertyValue)
      {
         propertyName = _propertyName;
         propertyValue = _propertyValue;
      }
   }
}
