using LogMagic.Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogMagic
{
   public static class ILogConfigurationExtensions
   {
      public static ILoggerProvider CreateNetCoreLoggerProvider(this ILogConfiguration config)
      {
         return new LogMagicLoggerProvider();
      }
   }
}
