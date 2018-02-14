using Microsoft.ServiceFabric.Services.Common;
using Microsoft.ServiceFabric.Services.Remoting.V2;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace LogMagic.Microsoft.Azure.ServiceFabric.Remoting
{
   /// <summary>
   /// As SF uses IDs to indicate which interface and method is called, this class can translate them to human-readable form.
   /// It's using code extracted from SF SDK as it's not available publically (better than using reflection which is slow).
   /// </summary>
   static class MethodResolver
   {
      class MethodEntry
      {
         public MethodInfo Method { get; set; }

         public MethodEntry(MethodInfo mi)
         {
            Method = mi;
         }
      }

      class InterfaceEntry
      {
         public Type InterfaceType { get; set; }

         public Dictionary<int, MethodEntry> Methods { get; } = new Dictionary<int, MethodEntry>();

         public InterfaceEntry(Type interfaceType)
         {
            InterfaceType = interfaceType;
         }
      }

      private static IDictionary<int, InterfaceEntry> idToMethodNameMap = new ConcurrentDictionary<int, InterfaceEntry>();

      public static string GetMethodName(int interfaceId, int methodId)
      {
         if (idToMethodNameMap.TryGetValue(interfaceId, out InterfaceEntry ifc))
         {
            if (ifc.Methods.TryGetValue(methodId, out MethodEntry method))
            {
               return $"{ifc.InterfaceType.Name}::{method.Method.Name}";
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
            if (!idToMethodNameMap.TryGetValue(interfaceIdv1, out InterfaceEntry ifc))
            {
               // Since idToMethodNameMap can be accessed by multiple threads, it is important to make sure
               // the inner dictionary has everything added, before this is added to idToMethodNameMap. The
               // inner dictionary will never be thread safe and it doesn't need to be, as long as it always
               // is effectively "read-only". If the order is reverse, you risk having another thread trying
               // to fetch a method from it prematurely.
               ifc = new InterfaceEntry(interfaceType);
               foreach (MethodInfo method in interfaceType.GetMethods())
               {
                  int methodIdv1 = IdUtil.ComputeId(method);
                  int methodIdv2 = IdUtil.ComputeIdWithCRC(method);

                  ifc.Methods[methodIdv1] = new MethodEntry(method);
                  ifc.Methods[methodIdv2] = new MethodEntry(method);
               }

               // If multiple threads are trying to set this entry, the last one wins, and this is ok to have
               // since this method map should always look the same once it's constructed.
               idToMethodNameMap[interfaceIdv1] = ifc;
               idToMethodNameMap[interfaceIdv2] = ifc;
            }
         }
      }
   }
}
