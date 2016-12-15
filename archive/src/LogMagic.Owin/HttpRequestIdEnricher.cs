using System.Web;

namespace LogMagic.Owin
{
   class HttpRequestIdEnricher : IEnricher
   {
      public void Enrich(LogEvent e, out string propertyName, out object propertyValue)
      {
         propertyName = "requestId";

         if (HttpContext.Current != null && HttpContext.Current.Items.Contains(LogMagicMiddleware.RequestIdHeaderName))
         {
            propertyValue = HttpContext.Current.Items[LogMagicMiddleware.RequestIdHeaderName]?.ToString();
         }
         else
         {
            propertyValue = null;
         }
      }
   }
}
