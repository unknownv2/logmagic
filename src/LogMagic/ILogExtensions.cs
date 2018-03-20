using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogMagic
{
   /// <summary>
   /// Useful logging extensions
   /// </summary>
   public static class ILogExtensions
   {
      public static void Dependency(this ILog log, string type, string name, string command, long duration,
         Exception error = null,
         params string[] properties)
      {
         log.Dependency(type, name, command, duration, error, Compress(properties));
      }

      public static void Request(this ILog log, string name, long duration, Exception error = null, params string[] properties)
      {
         log.Request(name, duration, error, Compress(properties));
      }

      private static Dictionary<string, object> Compress(params string[] properties)
      {
         var d = new Dictionary<string, object>();

         int maxLength = properties.Length - properties.Length % 2;
         for (int i = 0; i < maxLength; i += 2)
         {
            d[properties[i]] = properties[i + 1];
         }

         return d;
      }
   }
}
