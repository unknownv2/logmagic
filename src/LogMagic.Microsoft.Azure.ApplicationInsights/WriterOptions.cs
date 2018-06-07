using System;
using System.Collections.Generic;
using System.Text;

namespace LogMagic.Microsoft.Azure.ApplicationInsights
{
   class WriterOptions
   {
      public bool FlushOnWrite { get; set; }

      public bool TraceExceptions { get; set; }
   }
}
