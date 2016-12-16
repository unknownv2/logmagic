using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin;

namespace LogMagic.Owin
{
   public class LogMagicMiddleware : OwinMiddleware
    {
      private readonly ILog _log = L.G();
      public const string RequestIdHeaderName = "request-id";


      public LogMagicMiddleware(OwinMiddleware next) : base(next)
      {
      }

      public override async Task Invoke(IOwinContext context)
      {
         IOwinRequest request = context.Request;
         IOwinResponse response = context.Response;

         //this doesn't work in API when user object is not strictly initialised
         /*string userName = (request.User != null && request.User.Identity.IsAuthenticated)
            ? request.User.Identity.Name
            : "anonymous";*/

         SetRequestId(request);

         _log.D("=> {requestMethod} {requestUri}...", request.Method, request.Uri);

         var stopwatch = new Stopwatch();
         stopwatch.Start();
         try
         {
            await Next.Invoke(context);
         }
         catch (Exception ex)
         {
            stopwatch.Stop();
            _log.D("<= {statusCode} {requestMethod} {requestUri} ({time}ms).",
               response.StatusCode, request.Method, request.Uri, stopwatch.ElapsedMilliseconds, ex);
            throw;
         }

         stopwatch.Stop();

         _log.D("<= {requestMethod} {requestUri} ({time}ms).",
               request.Method, request.Uri, stopwatch.ElapsedMilliseconds);
      }

      private void SetRequestId(IOwinRequest request)
      {
         string requestId;

         string[] requestIds;
         if (request.Headers.TryGetValue(RequestIdHeaderName, out requestIds) && requestIds != null && requestIds.Length > 0)
         {
            requestId = requestIds[0];
         }
         else
         {
            requestId = Guid.NewGuid().ToString();
         }

         HttpContext.Current.Items[RequestIdHeaderName] = requestId;
      }
   }
}
