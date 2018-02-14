using Microsoft.ServiceFabric.Services.Common;
using Microsoft.ServiceFabric.Services.Remoting.V2;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace LogMagic.Microsoft.Azure.ServiceFabric.Remoting
{
   static class MethodResolver
   {
      private static IDictionary<int, Dictionary<int, string>> idToMethodNameMap = new ConcurrentDictionary<int, Dictionary<int, string>>();

      public static string GetMethodName(int interfaceId, int methodId)
      {
         if (idToMethodNameMap.TryGetValue(interfaceId, out Dictionary<int, string> methodMap))
         {
            if (methodMap.TryGetValue(methodId, out string methodName))
            {
               return methodName;
            }
         }

         return $"{interfaceId}.{methodId}";
      }

      public static string GetMethodName(IServiceRemotingRequestMessage message)
      {
         IServiceRemotingRequestMessageHeader header = message.GetHeader();

         return GetMethodName(header.InterfaceId, header.MethodId);
      }

      public static void AddMethodsForProxyOrService(IEnumerable<Type> interfaces, Type baseInterfaceType)
      {
         foreach (Type interfaceType in interfaces)
         {
            if (!baseInterfaceType.IsAssignableFrom(interfaceType) || interfaceType == baseInterfaceType)
            {
               continue;
            }

            int interfaceIdv1 = IdUtil.ComputeId(interfaceType);
            int interfaceIdv2 = IdUtil.ComputeIdWithCRC(interfaceType);

            // Add if it's not there, don't add if it's there already
            if (!idToMethodNameMap.TryGetValue(interfaceIdv1, out Dictionary<int, string> methodMap))
            {
               // Since idToMethodNameMap can be accessed by multiple threads, it is important to make sure
               // the inner dictionary has everything added, before this is added to idToMethodNameMap. The
               // inner dictionary will never be thread safe and it doesn't need to be, as long as it always
               // is effectively "read-only". If the order is reverse, you risk having another thread trying
               // to fetch a method from it prematurely.
               methodMap = new Dictionary<int, string>();
               foreach (MethodInfo method in interfaceType.GetMethods())
               {
                  int methodIdv1 = IdUtil.ComputeId(method);
                  int methodIdv2 = IdUtil.ComputeIdWithCRC(method);

                  string methodName = $"{interfaceType.Name}::{method.Name}";

                  methodMap[methodIdv1] = methodName;
                  methodMap[methodIdv2] = methodName;
               }

               // If multiple threads are trying to set this entry, the last one wins, and this is ok to have
               // since this method map should always look the same once it's constructed.
               idToMethodNameMap[interfaceIdv1] = methodMap;
               idToMethodNameMap[interfaceIdv2] = methodMap;
            }
         }
      }
   }
}
