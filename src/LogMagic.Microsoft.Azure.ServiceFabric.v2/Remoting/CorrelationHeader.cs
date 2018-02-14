using System;
using System.Collections.Generic;
using LogMagic.Enrichers;
using Microsoft.ServiceFabric.Services.Remoting;

namespace LogMagic.Microsoft.Azure.ServiceFabric.Remoting
{
   static class CorrelationHeader
   {
      private const string Prefix = "x-logmagic-";
      public static string HeaderListHeaderName = Prefix + "headerList";

      public static string MakeHeader(string name)
      {
         return Prefix + name;
      }

      public static string ParseHeader(string name)
      {
         if (name.StartsWith(Prefix)) return name.Substring(Prefix.Length);

         return name;
      }

      public static Dictionary<string, string> ParseHeaders(
         ServiceRemotingMessageHeaders messageHeaders,
         out string operationId)
      {
         var result = new Dictionary<string, string>();

         if (messageHeaders.TryGetHeaderValue(HeaderListHeaderName, out string listString))
         {
            string[] list = listString.Split(';');

            foreach(string name in list)
            {
               if(messageHeaders.TryGetHeaderValue(CorrelationHeader.MakeHeader(name), out string value))
               {
                  result[name] = value;
               }
            }
         }

         if(result.TryGetValue(KnownProperty.OperationId, out string operationIdString))
         {
            operationId = operationIdString;
            result.Remove(KnownProperty.OperationId);
         }
         else
         {
            operationId = Guid.NewGuid().ToString();
         }

         return result;
      }
   }
}
