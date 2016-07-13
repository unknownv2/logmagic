namespace LogMagic
{
   /// <summary>
   /// Configuration of logging enrichers
   /// </summary>
   public interface IEnricherConfiguration
   {
      /// <summary>
      /// Adds a custom enricher
      /// </summary>
      ILogConfiguration Custom(IEnricher enricher);
   }
}
