using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LogMagic
{
   /// <summary>
   /// Useful logging extensions
   /// </summary>
   public static class ILogExtensions
   {
      /// <summary>
      /// Track dependency automatically, measure time and handle exceptions
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="log">The log.</param>
      /// <param name="type">The type.</param>
      /// <param name="name">The name.</param>
      /// <param name="command">The command.</param>
      /// <param name="call">The call.</param>
      /// <param name="properties">Extra properties</param>
      public static async Task Dependency(this ILog log, string type, string name, string command, Func<Task> call, Dictionary<string, object> properties = null)
      {
         using (var time = new TimeMeasure())
         {
            try
            {
               await call();
               log.Dependency(type, name, command, time.ElapsedTicks, properties, null);
            }
            catch (Exception ex)
            {
               log.Dependency(type, name, command, time.ElapsedTicks, properties, ex);
               throw;
            }
         }

      }
   }
}
