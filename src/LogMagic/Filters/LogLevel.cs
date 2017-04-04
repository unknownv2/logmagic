using System;

namespace LogMagic.Filters
{
   class LogSeverityLevel : IFilter
   {
      private readonly LogSeverity _minSeverity;

      public LogSeverityLevel(LogSeverity minSeverity)
      {
         _minSeverity = minSeverity;
      }

      public bool Match(LogEvent e)
      {
         return (int)e.Severity >= (int)_minSeverity;
      }
   }
}
