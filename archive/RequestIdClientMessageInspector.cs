using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace LogMagic.WCF
{
   class RequestIdClientMessageInspector : IClientMessageInspector
   {
      public void AfterReceiveReply(ref Message reply, object correlationState)
      {
      }

      public object BeforeSendRequest(ref Message request, IClientChannel channel)
      {
         /*if (!request.Properties.ContainsKey("request-id"))
         {
            if (HttpContext.Current.Items.Contains("request-id"))
            {
               string requestId = HttpContext.Current.Items["request-id"].ToString();

               var wcfHeader = new MessageHeader<string>(requestId);
               var untyped = wcfHeader.GetUntypedHeader("request-id", "http://hfea.gov.uk");
               request.Headers.Add(untyped);
            }
         }*/

         return null;
      }
   }
}
