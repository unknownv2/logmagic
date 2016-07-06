using LogMagic.Enrichers;

namespace LogMagic
{
   public static class EnricherExtensions
   {
      public static ILogConfiguration EnrichWithThreadId(this ILogConfiguration configuration)
      {
         return configuration.AddEnricher(new ThreadInfoEnricher());
      }

      public static ILogConfiguration EnrichWithConstant(this ILogConfiguration configuration,
         string propertyName,
         string propertyValue)
      {
         return configuration.AddEnricher(new ConstantEnricher(propertyName, propertyValue));
      }
   }
}
