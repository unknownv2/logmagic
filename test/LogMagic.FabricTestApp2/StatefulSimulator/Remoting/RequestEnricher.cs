using LogMagic;
using Microsoft.ServiceFabric.Services.Remoting.V2;
using System;
using System.Collections.Generic;
using System.Text;

namespace StatefulSimulator.Remoting
{
   class RequestEnricher
   {
      private static readonly Encoding Enc = Encoding.UTF8;

      public void Enrich(IServiceRemotingRequestMessage message)
      {
         Dictionary<string, string> context = L.GetContextValues();
         if (context.Count == 0) return;

         IServiceRemotingRequestMessageHeader headers = message.GetHeader();

         foreach(var cv in context)
         {
            headers.AddHeader(cv.Key, GetHeaderValue(cv.Value));
         }
      }

      private static byte[] GetHeaderValue(string s)
      {
         if (s == null) return null;

         return Enc.GetBytes(s);
      }
   }
}
