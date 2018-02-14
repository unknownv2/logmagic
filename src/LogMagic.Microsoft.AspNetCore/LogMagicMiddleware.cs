using System;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Http;

namespace LogMagic.Microsoft.AspNetCore
{
   class LogMagicMiddleware
   {
      private static readonly ILog log = L.G(typeof(LogMagicMiddleware));
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

         if (appInsightsTelemetry != null) return appInsightsTelemetry.Context.Operation.Id;

         return null;
      }

   }
}
