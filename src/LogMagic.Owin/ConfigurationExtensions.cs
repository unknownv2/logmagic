using LogMagic.Owin;

namespace LogMagic
{
   public static class ConfigurationExtensions
   {
      public static ILogConfiguration HttpRequestId(this IEnricherConfiguration configuration)
      {
         return configuration.Custom(new HttpRequestIdEnricher());
      }
   }
}
