using LogMagic.Enrichers;

namespace LogMagic
{
   public static class EnricherConfigurationExtensions
   {
      public static ILogConfiguration ThreadId(this IEnricherConfiguration configuration)
      {
         return configuration.Custom(new ThreadInfoEnricher());
      }

      public static ILogConfiguration Constant(this IEnricherConfiguration configuration,
         string propertyName,
         string propertyValue)
      {
         return configuration.Custom(new ConstantEnricher(propertyName, propertyValue));
      }

#if !PORTABLE
      public static ILogConfiguration MethodName(this IEnricherConfiguration configuration)
      {
         return configuration.Custom(new MethodNameEnricher());
      }
#endif
   }
}
