using LogMagic.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogMagic
{
   public static class FiltersConfigurationExtensions
   {
      public static ILogConfiguration Lambda(this IFilterConfiguration configuration, Func<LogEvent, bool> func)
      {
         return configuration.Custom(new LambdaFilter(func));
      }
   }
}
