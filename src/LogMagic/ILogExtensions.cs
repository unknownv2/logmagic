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
      public static async Task Dependency(this ILog log, string type, string name, string command, Func<Task> call, params KeyValuePair<string, object>[] properties)
      {
         using (var time = new TimeMeasure())
         {
            try
            {
               await call();
               log.Dependency(type, name, command, time.ElapsedTicks, null, properties);
            }
            catch (Exception ex)
            {
               log.Dependency(type, name, command, time.ElapsedTicks, ex, properties);
               throw;
            }
         }
      }

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
      public static async Task<T> Dependency<T>(this ILog log, string type, string name, string command, Func<Task<T>> call, params KeyValuePair<string, object>[] properties)
      {
         using (var time = new TimeMeasure())
         {
            try
            {
               T result = await call();
               log.Dependency(type, name, command, time.ElapsedTicks, null, properties);
               return result;
            }
            catch (Exception ex)
            {
               log.Dependency(type, name, command, time.ElapsedTicks, ex, properties);
               throw;
            }
         }
      }

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
      public static T Dependency<T>(this ILog log, string type, string name, string command, Func<T> call, params KeyValuePair<string, object>[] properties)
      {
         using (var time = new TimeMeasure())
         {
            try
            {
               T result = call();
               log.Dependency(type, name, command, time.ElapsedTicks, null, properties);
               return result;
            }
            catch (Exception ex)
            {
               log.Dependency(type, name, command, time.ElapsedTicks, ex, properties);
               throw;
            }
         }
      }

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
      public static void Dependency(this ILog log, string type, string name, string command, Action call, params KeyValuePair<string, object>[] properties)
      {
         using (var time = new TimeMeasure())
         {
            try
            {
               call();
               log.Dependency(type, name, command, time.ElapsedTicks, null, properties);
            }
            catch (Exception ex)
            {
               log.Dependency(type, name, command, time.ElapsedTicks, ex, properties);
               throw;
            }
         }
      }

      public static void Request(this ILog log, string name, Action call, params KeyValuePair<string, object>[] properties)
      {
         using (var time = new TimeMeasure())
         {
            try
            {
               call();
               log.Request(name, time.ElapsedTicks, null, properties);
            }
            catch(Exception ex)
            {
               log.Request(name, time.ElapsedTicks, ex, properties);
               throw;
            }
         }
      }
   }
}
