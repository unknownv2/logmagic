namespace LogMagic
{
   /// <summary>
   /// Specification for enriching log events
   /// </summary>
   public interface IEnricher
   {
      /// <summary>
      /// Called by the framework when logging event occurs
      /// </summary>
      /// <param name="e">Log event</param>
      /// <param name="propertyName">Property name to enrich</param>
      /// <param name="propertyValue">Enriched property value</param>
      void Enrich(LogEvent e, out string propertyName, out object propertyValue);
   }
}
