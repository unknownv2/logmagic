using System;
using System.Collections.Generic;
using LogMagic.Enrichers;

namespace LogMagic
{
   /// <summary>
   /// Useful logging extensions
   /// </summary>
   public static class ILogExtensions
   {
      public static void Dependency(this ILog log, string type, string name, string command, long duration,
         Exception error = null,
         params object[] properties)
      {
         log.Dependency(type, name, command, duration, error, Compress(properties));
      }

      public static void Request(this ILog log, string name, long duration, Exception error = null, params object[] properties)
      {
         log.Request(name, duration, error, Compress(properties));
      }

      public static void Event(this ILog log, string name, params object[] properties)
      {
         log.Event(name, Compress(properties));
      }

      /// <summary>
      /// Helper method to trace in <see cref="LogSeverity.Verbose"/> level
      /// </summary>
      public static void Debug(this ILog log, string format, params object[] parameters)
      {
         log.Trace(format, parameters, Compress(KnownProperty.Severity, LogSeverity.Verbose));
      }

      /// <summary>
      /// Helper method to trace in <see cref="LogSeverity.Information"/> level
      /// </summary>
      public static void Info(this ILog log, string format, params object[] parameters)
      {
         log.Trace(format, parameters, Compress(KnownProperty.Severity, LogSeverity.Information));
      }

      /// <summary>
      /// Helper method to trace in <see cref="LogSeverity.Warning"/> level
      /// </summary>
      public static void Warn(this ILog log, string format, params object[] parameters)
      {
         log.Trace(format, parameters, Compress(KnownProperty.Severity, LogSeverity.Warning));
      }

      /// <summary>
      /// Helper method to trace in <see cref="LogSeverity.Error"/> level
      /// </summary>
      public static void Error(this ILog log, string format, params object[] parameters)
      {
         log.Trace(format, parameters, Compress(KnownProperty.Severity, LogSeverity.Error));
      }

      /// <summary>
      /// Helper method to trace in <see cref="LogSeverity.Critical"/> level
      /// </summary>
      public static void Critical(this ILog log, string format, params object[] parameters)
      {
         log.Trace(format, parameters, Compress(KnownProperty.Severity, LogSeverity.Critical));
      }

      private static Dictionary<string, object> Compress(params object[] properties)
      {
         var d = new Dictionary<string, object>();

         int maxLength = properties.Length - properties.Length % 2;
         for (int i = 0; i < maxLength; i += 2)
         {
            object keyObj = properties[i];
            if (!(keyObj is string key)) throw new ArgumentOutOfRangeException($"{nameof(properties)}[{i}]", "parameter must be of string type");

            d[key] = properties[i + 1];
         }

         return d;
      }
   }
}
