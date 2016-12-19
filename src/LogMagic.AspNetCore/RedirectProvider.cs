using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogMagic.AspNetCore
{
   class RedirectProvider : ILoggerProvider
   {
      public RedirectProvider()
      {
         L.Config.WriteTo.Custom()
      }

      public ILogger CreateLogger(string categoryName)
      {
         return new LoggerAdapter(L.G(categoryName));
      }

      public void Dispose()
      {
      }

      private class LoggerAdapter : ILogger
      {
         private readonly ILog _log;

         public LoggerAdapter(ILog log)
         {
            _log = log;
         }

         public IDisposable BeginScope<TState>(TState state)
         {
            return null;
         }

         public bool IsEnabled(LogLevel logLevel)
         {
            return true;
         }

         public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
         {
            L.Flush

            throw new NotImplementedException();
         }
      }
   }
}