using LogMagic.Filters;

namespace LogMagic
{
   public static class FiltersConfigurationExtensions
   {
      public static ILogConfiguration MinSeverity(this IFilterConfiguration configuration, LogSeverity minSeverity)
      {
         return configuration.Custom(new LogSeverityLevel(minSeverity));
      }
   }
}
