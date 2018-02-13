using LogMagic;
using Microsoft.ServiceFabric.Services.Remoting.V2;
using System;
using System.Collections.Generic;
using System.Text;

namespace StatefulSimulator.Remoting
{
   class RequestEnricher
   {
      public void Enrich(IServiceRemotingRequestMessage message)
      {
         Dictionary<string, string> context = L.GetContextValues();
         if (context.Count == 0) return;

         IServiceRemotingRequestMessageHeader headers = message.GetHeader();

         //headers.AddHeader()
      }
   }
}
