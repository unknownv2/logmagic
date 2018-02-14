using System;
using System.Reflection;
using System.Text;

namespace Microsoft.ServiceFabric.Services.Common
{
   internal static class IdUtil
   {
      internal static int ComputeId(MethodInfo methodInfo)
      {
         int currentKey = methodInfo.Name.GetHashCode();
         if (methodInfo.DeclaringType != (Type)null)
         {
            if (methodInfo.DeclaringType.Namespace != null)
               currentKey = IdUtil.HashCombine(methodInfo.DeclaringType.Namespace.GetHashCode(), currentKey);
            currentKey = IdUtil.HashCombine(methodInfo.DeclaringType.Name.GetHashCode(), currentKey);
         }
         return currentKey;
      }

      internal static int ComputeId(Type type)
      {
         int currentKey = type.Name.GetHashCode();
         if (type.Namespace != null)
            currentKey = IdUtil.HashCombine(type.Namespace.GetHashCode(), currentKey);
         return currentKey;
      }

      internal static int ComputeIdWithCRC(Type type)
      {
         string typeName = type.Name;
         if (type.Namespace != null)
            typeName = type.Namespace + typeName;
         return IdUtil.ComputeIdWithCRC(typeName);
      }

      internal static int ComputeIdWithCRC(MethodInfo methodInfo)
      {
         string typeName = methodInfo.Name;
         if (methodInfo.DeclaringType != (Type)null)
         {
            if (methodInfo.DeclaringType.Namespace != null)
               typeName = methodInfo.DeclaringType.Namespace + typeName;
            typeName = methodInfo.DeclaringType.Name + typeName;
         }
         return IdUtil.ComputeIdWithCRC(typeName);
      }

      internal static int ComputeIdWithCRC(string typeName)
      {
         return (int)CRC64.ToCRC64(Encoding.UTF8.GetBytes(typeName));
      }

      internal static int ComputeId(string typeName, string typeNamespace)
      {
         int currentKey = typeName.GetHashCode();
         if (typeNamespace != null)
            currentKey = IdUtil.HashCombine(typeNamespace.GetHashCode(), currentKey);
         return currentKey;
      }

      /// <summary>
      /// This is how VB Anonymous Types combine hash values for fields.
      /// </summary>
      internal static int HashCombine(int newKey, int currentKey)
      {
         return currentKey * -1521134295 + newKey;
      }
   }
}
