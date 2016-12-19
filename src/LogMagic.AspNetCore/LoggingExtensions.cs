using LogMagic.AspNetCore;

namespace Microsoft.Extensions.Logging
{
   public static class LoggingExtensions
   {
      /// <summary>
      /// Redirects ASP.NET CORE logging events to LogMagic
      /// </summary>
      /// <param name="factory"></param>
      /// <returns></returns>
      public static ILoggerFactory UseLogMagic(this ILoggerFactory factory)
      {
         factory.AddProvider(new RedirectProvider());

         return factory;
      }
   }
}
