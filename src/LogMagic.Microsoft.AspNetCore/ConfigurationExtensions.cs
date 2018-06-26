using LogMagic.Microsoft.AspNetCore;

namespace Microsoft.AspNetCore.Builder
{
   public static class ConfigurationExtensions
   {
      /// <summary>
      /// Enable LogMagic automatic tracing for ASP.NET Core.
      /// Important!!! Make sure to call the UseElmahIo-method after installation of other pieces of middleware handling exceptions (like UseDeveloperExceptionPage and UseExceptionHandler), but before any calls to UseStaticFiles, UseMvc and similar.
      /// </summary>
      public static IApplicationBuilder UseLogMagic(this IApplicationBuilder app)
      {
         return app.UseMiddleware<LogMagicMiddleware>();
      }
   }
}