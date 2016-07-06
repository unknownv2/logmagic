using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogMagic.Enrichers
{
#if !PORTABLE
   class MethodNameEnricher : IEnricher
   {
      public void Enrich(LogEvent e, out string propertyName, out string propertyValue)
      {
         var frame = new StackFrame();

         throw new NotImplementedException();
      }
   }
#endif
}
