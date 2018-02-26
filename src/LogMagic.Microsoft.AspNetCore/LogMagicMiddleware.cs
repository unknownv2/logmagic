using System;
using System.Threading.Tasks;
using LogMagic.Enrichers;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Http;

namespace LogMagic.Microsoft.AspNetCore
{
   class LogMagicMiddleware
   {
      private static readonly ILog log = L.G(typeof(LogMagicMiddleware));
      private static bool _appInsightsInitialised;
      private readonly RequestDelegate _next;

      public LogMagicMiddleware(RequestDelegate next)
      {
         _next = next;
      }

      public Task Invoke(HttpContext context)
      {
         using (var time = new TimeMeasure())
         {
            string name = $"{context.Request.Path}{context.Request.QueryString}";
            string operationId = TryGetOperationId(context);

            using (L.Operation(operationId))
            {
               return _next(context);
            }
         }
      }

      private string TryGetOperationId(HttpContext context)
      {
         RequestTelemetry appInsightsTelemetry = context.Features.Get<RequestTelemetry>();

         if (appInsightsTelemetry != null)
         {
            if(!_appInsightsInitialised)
            {
               string roleName = appInsightsTelemetry.Context.Cloud?.RoleName;
               string roleInstance = appInsightsTelemetry.Context.Cloud?.RoleInstance;

               if (roleName != null) L.Config.EnrichWith.Constant(KnownProperty.RoleName, roleName);
               if (roleInstance != null) L.Config.EnrichWith.Constant(KnownProperty.RoleInstance, roleInstance);

               _appInsightsInitialised = true;
            }

            return appInsightsTelemetry.Context.Operation.Id;
         }

         return null;
      }

   }
}
