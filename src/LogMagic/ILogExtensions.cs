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
      /// <returns></returns>
      public static async Task<T> Dependency<T>(this ILog log, string type, string name, string command, Func<Task<T>> call)
      {
         using (var time = new TimeMeasure())
         {
            try
            {
               T result = await call();
               log.Dependency(type, name, command, time.ElapsedTicks);
               return result;
            }
            catch (Exception ex)
            {
               log.Dependency(type, name, command, time.ElapsedTicks, ex);
               throw;
            }
         }

      }
   }
}
