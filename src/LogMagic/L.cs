using System;
using System.Collections.Generic;

namespace LogMagic
{
   /// <summary>
   /// Global public logging configuration and initialisation class
   /// </summary>
   public static class L
   {
      private static readonly LogContainer _default = new LogContainer();


      /// <summary>
      /// Gets logging library configuration
      /// </summary>
      public static ILogConfiguration Config => _default.Config;

      /// <summary>
      /// Default logging container
      /// </summary>
      public static LogContainer DefaultContainer => _default;

      /// <summary>
      /// Get logger for the specified type
      /// <typeparam name="T">Class type</typeparam>
      /// </summary>
      public static ILog G<T>()
      {
         return _default.G<T>();
      }

      /// <summary>
      /// Get logger for the specified type
      /// </summary>
      public static ILog G(Type t)
      {
         return _default.G(t);
      }

      /// <summary>
      /// Gets logger by specified name. Use when you can't use more specific methods.
      /// </summary>
      public static ILog G(string name)
      {
         return _default.G(name);
      }

#if !NET45

      /// <summary>
      /// Adds one or more context properties.
      /// </summary>
      /// <param name="properties">
      /// Array or properties where even numbers are property names and odd numbers are property values.
      /// If you have an odd number of array elements the last one is discarded.
      /// </param>
      public static IDisposable Context(params string[] properties)
      {
         return _default.Context(properties);
      }

      /// <summary>
      /// Adds a context property
      /// </summary>
      public static IDisposable Context(Dictionary<string, string> properties)
      {
         return _default.Context(properties);
      }

      /// <summary>
      /// Gets a context property by name
      /// </summary>
      /// <param name="propertyName">Property name</param>
      /// <returns></returns>
      public static string GetContextValue(string propertyName)
      {
         return _default.GetContextValue(propertyName);
      }

      /// <summary>
      /// Gets a dictionary of all current context values
      /// </summary>
      /// <returns></returns>
      public static Dictionary<string, string> GetContextValues()
      {
         return _default.GetContextValues();
      }

#endif

   }
}
