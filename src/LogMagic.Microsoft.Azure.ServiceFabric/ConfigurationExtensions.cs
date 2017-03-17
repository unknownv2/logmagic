using LogMagic.Microsoft.Azure.ServiceFabric.Enrichers;
using System.Fabric;

namespace LogMagic
{
   public static class ConfigurationExtensions
   {
      public static ILogConfiguration AzureServiceFabricContext(this IEnricherConfiguration configuration, ServiceContext context)
      {
         return configuration.Custom(new ServiceFabricEnricher(context));
      }
   }
}
