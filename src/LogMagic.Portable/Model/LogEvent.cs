using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogMagic.Model
{
   public class LogEvent
   {
      public LogEvent()
      {

      }

      public string SourceName;

      public LogSeverity Severity;

      public DateTime EventTime;

      public Exception Error;

      public Dictionary<string, string> Properties;
   }
}
