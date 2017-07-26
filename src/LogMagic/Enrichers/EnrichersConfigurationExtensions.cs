using LogMagic.Enrichers;

namespace LogMagic
{
   /// <summary>
   /// Extension methods to easily add enrichers
   /// </summary>
   public static class EnricherConfigurationExtensions
   {
      /// <summary>
      /// Enrich with managed thread id
      /// </summary>
      public static ILogConfiguration ThreadId(this IEnricherConfiguration configuration)
      {
         return configuration.Custom(new ThreadInfoEnricher());
      }

      /// <summary>
      /// Enrich with a consatant value which will always be added to every log event
      /// </summary>
      public static ILogConfiguration Constant(this IEnricherConfiguration configuration,
         string propertyName,
         string propertyValue)
      {
         return configuration.Custom(new ConstantEnricher(propertyName, propertyValue));
      }

#if NETFULL
      /// <summary>
      /// Enrich with caller's method name. This is using reflection on every log event therefore
      /// call with caution as it may cause serious performance issues in production.
      /// </summary>
      public static ILogConfiguration MethodName(this IEnricherConfiguration configuration)
      {
         return configuration.Custom(new MethodNameEnricher());
      }
#endif

#if !NETSTANDARD14
      /// <summary>
      /// Enrich with current machine name
      /// </summary>
      public static ILogConfiguration MachineName(this IEnricherConfiguration configuration)
      {
         return configuration.Custom(new MachineNameEnricher());
      }
#endif

      /// <summary>
      /// Enrich by this machine's IP address
      /// </summary>
      public static ILogConfiguration MachineIpAddress(this IEnricherConfiguration configuration, bool includeIpV6 = false)
      {
         return configuration.Custom(new MachineIpEnricher(includeIpV6));
      }
   }
}
