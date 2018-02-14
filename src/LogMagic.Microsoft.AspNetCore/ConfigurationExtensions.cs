using LogMagic.Microsoft.AspNetCore;

namespace Microsoft.AspNetCore.Builder
{
   public static class ConfigurationExtensions
   {
      public static IApplicationBuilder UseLogMagic(this IApplicationBuilder app)
      {
         return app.UseMiddleware<LogMagicMiddleware>();
      }
   }
}