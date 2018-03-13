using LogMagic.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogMagic
{
   /// <summary>
   /// Contains extensions for configuring logging filters
   /// </summary>
   public static class FiltersConfigurationExtensions
   {
      /// <summary>
      /// Applies a lambda function
      /// </summary>
      /// <param name="configuration"></param>
      /// <param name="func"></param>
      /// <returns></returns>
      public static ILogConfiguration Lambda(this IFilterConfiguration configuration, Func<LogEvent, bool> func)
      {
         return configuration.Custom(new LambdaFilter(func));
      }
   }
}
