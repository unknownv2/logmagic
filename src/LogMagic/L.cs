using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using LogMagic.Configuration;
using LogMagic.Enrichers;

namespace LogMagic
{
   /// <summary>
   /// Global public logging configuration and initialisation class
   /// </summary>
   public static class L
   {

      /// <summary>
      /// Gets logging library configuration
      /// </summary>
      public static ILogConfiguration Config { get; } = new LogConfiguration();
      /// <summary>
      /// Get logger for the specified type
      /// <typeparam name="T">Class type</typeparam>
      /// </summary>
      public static ILog G<T>()
      {
         return new LogClient(typeof(T));
      }

      /// <summary>
      /// Get logger for the specified type
      /// </summary>
      public static ILog G(Type t)
      {
         return new LogClient(t);
      }

      /// <summary>
      /// Gets logger by specified name. Use when you can't use more specific methods.
      /// </summary>
      public static ILog G(string name)
      {
         return new LogClient(name);
      }

#if NETFULL
    /// <summary>
    /// Get logger for the current class
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.NoInlining)]
      public static ILog G()
      {
         return new LogClient(GetClassFullName());
      }
#endif

#if NETFULL
    /// <summary>
    /// Gets the fully qualified name of the class invoking the LogManager, including the 
    /// namespace but not the assembly.    
    /// </summary>
    [MethodImpl(MethodImplOptions.NoInlining)]
      private static string GetClassFullName()
      {
         string className;
         Type declaringType;
         int framesToSkip = 2;

         do
         {
            StackFrame frame = new StackFrame(framesToSkip, false);
            MethodBase method = frame.GetMethod();
            declaringType = method.DeclaringType;
            if (declaringType == null)
            {
               className = method.Name;
               break;
            }

            framesToSkip++;
            className = declaringType.FullName;
         } while (declaringType.Module.Name.Equals("mscorlib.dll", StringComparison.OrdinalIgnoreCase));

         return className;
      }
#endif


#if !NET45

      /// <summary>
      /// Adds a context property
      /// </summary>
      public static IDisposable Context(IEnumerable<KeyValuePair<string, string>> properties)
      {
         if (properties == null) return null;

         return LogContext.Push(properties);
      }

      /// <summary>
      /// Adds a context property
      /// </summary>
      public static IDisposable Context(params KeyValuePair<string, string>[] properties)
      {
         if (properties == null || properties.Length == 0) return null;

         return LogContext.Push(properties);
      }

      /// <summary>
      /// Gets a context property by name
      /// </summary>
      /// <param name="propertyName">Property name</param>
      /// <returns></returns>
      public static string GetContextValue(string propertyName)
      {
         if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));

         return LogContext.GetValueByName(propertyName);
      }

      /// <summary>
      /// Gets a dictionary of all current context values
      /// </summary>
      /// <returns></returns>
      public static Dictionary<string, string> GetContextValues()
      {
         return LogContext.GetAllValues();
      }

      /// <summary>
      /// Marks a start of an operation pushing operation id to the current context. If context already contains operation id,
      /// then it's set as operation's parent ID
      /// </summary>
      /// <param name="id">ID of the operation. When ommitted a new uniqueue ID is generated</param>
      public static IDisposable Operation(Guid? id = null)
      {
         string existingId = L.GetContextValue(KnownProperty.OperationId);
         string operationId = id == null ? Guid.NewGuid().ToString() : id.Value.ToString();
         string parentId = existingId;

         var ps = new Dictionary<string, string>
         {
            [KnownProperty.OperationId] = operationId
         };
         if (parentId != null) ps[KnownProperty.OperationParentId] = parentId;

         return LogContext.Push(ps);
      }
#endif

   }
}
