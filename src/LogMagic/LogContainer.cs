using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LogMagic.Configuration;
using LogMagic.PerfCounters;

namespace LogMagic
{
   /// <summary>
   /// Logging container provides a logging system instance. The global <see cref="L"/> variable provides access
   /// to a default pre-created container, however you can create more containers if you app needs isolated
   /// logging containers with distincts configurations.
   /// </summary>
   public class LogContainer
   {
      private readonly PerfLoop _perfLoop;

#if !NET45
      private readonly LogContext _context;
#endif

      /// <summary>
      /// Creates an instance of logging container
      /// </summary>
      public LogContainer()
      {
         Config = new LogConfiguration();
#if !NET45
         _context = new LogContext();
#endif

         _perfLoop = new PerfLoop(new LogClient(Config, typeof(LogContainer).Name), Config);
      }

      /// <summary>
      /// Container configuration
      /// </summary>
      public ILogConfiguration Config { get; }

      /// <summary>
      /// Get logger for the specified type
      /// <typeparam name="T">Class type</typeparam>
      /// </summary>
      public ILog G<T>()
      {
         return new LogClient(Config, typeof(T).FullName);
      }

      /// <summary>
      /// Get logger for the specified type
      /// </summary>
      public ILog G(Type t)
      {
         return new LogClient(Config, t.FullName);
      }

      /// <summary>
      /// Gets logger by specified name. Use when you can't use more specific methods.
      /// </summary>
      public ILog G(string name)
      {
         return new LogClient(Config, name);
      }


#if !NET45
      /// <summary>
      /// Adds one or more context properties.
      /// </summary>
      /// <param name="properties">
      /// Array or properties where even numbers are property names and odd numbers are property values.
      /// If you have an odd number of array elements the last one is discarded.
      /// </param>
      public IDisposable Context(params string[] properties)
      {
         if (properties == null || properties.Length < 2) return null;

         var d = new Dictionary<string, string>();

         int maxLength = properties.Length - properties.Length % 2;
         for (int i = 0; i < maxLength; i += 2)
         {
            d[properties[i]] = properties[i + 1];
         }

         return _context.Push(d);
      }

      /// <summary>
      /// Adds a context property
      /// </summary>
      public IDisposable Context(Dictionary<string, string> properties)
      {
         if (properties == null || properties.Count == 0) return null;

         return _context.Push(properties);
      }

      /// <summary>
      /// Gets a context property by name
      /// </summary>
      /// <param name="propertyName">Property name</param>
      /// <returns></returns>
      public string GetContextValue(string propertyName)
      {
         if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));

         return _context.GetValueByName(propertyName);
      }

      /// <summary>
      /// Gets a dictionary of all current context values
      /// </summary>
      /// <returns></returns>
      public Dictionary<string, string> GetContextValues()
      {
         return _context.GetAllValues();
      }
#endif
   }
}
