using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LogMagic.Microsoft.Azure.ServiceFabric.Remoting
{
   class MethodNameProvider : IMethodNameProvider
   {
      private IDictionary<int, Dictionary<int, string>> _idToMethodNameMap;
      private bool _useConcurrentDictionary;

      public MethodNameProvider(bool threadSafe)
      {
         _useConcurrentDictionary = threadSafe;
         if (threadSafe)
         {
            _idToMethodNameMap = new ConcurrentDictionary<int, Dictionary<int, string>>();
         }
         else
         {
            _idToMethodNameMap = new Dictionary<int, Dictionary<int, string>>();
         }
      }

      public string GetMethodName(int interfaceId, int methodId)
      {
         if (_idToMethodNameMap.TryGetValue(interfaceId, out Dictionary<int, string> methodMap))
         {
            if (methodMap.TryGetValue(methodId, out string methodName))
            {
               return methodName;
            }
         }

         return null;
      }

      public void AddMethodsForProxyOrService(IEnumerable<Type> interfaces, Type baseInterfaceType)
      {
         foreach (Type interfaceType in interfaces)
         {
            if (!baseInterfaceType.IsAssignableFrom(interfaceType))
            {
               continue;
            }

            int interfaceId = IdUtilHelper.ComputeId(interfaceType);

            // Add if it's not there, don't add if it's there already
            if (!_idToMethodNameMap.TryGetValue(interfaceId, out Dictionary<int, string> methodMap))
            {
               // Since idToMethodNameMap can be accessed by multiple threads, it is important to make sure
               // the inner dictionary has everything added, before this is added to idToMethodNameMap. The
               // inner dictionary will never be thread safe and it doesn't need to be, as long as it always
               // is effectively "read-only". If the order is reverse, you risk having another thread trying
               // to fetch a method from it prematurely.
               methodMap = new Dictionary<int, string>();
               foreach (MethodInfo method in interfaceType.GetMethods())
               {
                  methodMap[IdUtilHelper.ComputeId(method)] = method.Name;
               }

               // If multiple threads are trying to set this entry, the last one wins, and this is ok to have
               // since this method map should always look the same once it's constructed.
               _idToMethodNameMap[interfaceId] = methodMap;
            }
         }
      }
   }
}
