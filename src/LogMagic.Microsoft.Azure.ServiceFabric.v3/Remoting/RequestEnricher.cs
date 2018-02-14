using LogMagic;
using Microsoft.ServiceFabric.Services.Remoting.V2;
using System.Collections.Generic;
using System.Text;

namespace LogMagic.Microsoft.Azure.ServiceFabric.Remoting
{
   class RequestEnricher
   {
      private static readonly Encoding Enc = Encoding.UTF8;

      public void Enrich(IServiceRemotingRequestMessage message)
      {
         Dictionary<string, string> context = L.GetContextValues();
         if (context.Count == 0) return;

         IServiceRemotingRequestMessageHeader headers = message.GetHeader();

         foreach(KeyValuePair<string, string> cv in context)
         {
            AddHeader(headers, cv);
         }
      }

      private static void AddHeader(IServiceRemotingRequestMessageHeader headers, KeyValuePair<string, string> header)
      {
         //don't add a header if it already exists
         //todo: figure out why it already exists
         if (headers.TryGetHeaderValue(header.Key, out byte[] headerValue)) return;

         byte[] value = header.Value == null ? null : Enc.GetBytes(header.Value);

         headers.AddHeader(header.Key, value);
      }
   }
}
