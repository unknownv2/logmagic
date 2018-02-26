using System;
using System.Reflection;
using Castle.DynamicProxy;
using LogMagic.Proxy;

namespace LogMagic
{
   public delegate void OnBeforeExecution(ILog log, MethodInfo method, object[] arguments);

   public delegate void OnAfterExecution(ILog log, MethodInfo method, object[] argument, object returnValue, long ticks, Exception error);

   public static class ConfigurationExtensions
   {
      private static ProxyGenerator PG = new ProxyGenerator();

      public static TInterface CreateInterfaceLogger<TInterface, TImplementation>(this ILog log, TImplementation instance,
         bool logExceptions = true,
         OnBeforeExecution onBeforeExecution = null,
         OnAfterExecution onAfterExecution = null)
         where TImplementation : TInterface
      {
         if (instance == null) throw new ArgumentNullException(nameof(instance));

         var interceptor = new LoggingInterceptor(log, logExceptions, onBeforeExecution, onAfterExecution);

         return (TInterface)PG.CreateInterfaceProxyWithTarget(typeof(TInterface), instance, interceptor);
      }
   }
}
