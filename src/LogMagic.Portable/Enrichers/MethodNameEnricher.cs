using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LogMagic.Enrichers
{
#if !PORTABLE
   class MethodNameEnricher : IEnricher
   {
      public void Enrich(LogEvent e, out string propertyName, out string propertyValue)
      {
         var frame = new StackFrame(4);   //warning! this can change after refactoring

         propertyName = "method";

         MethodBase method = frame.GetMethod();
         var sb = new StringBuilder();

         sb.Append(method.DeclaringType.FullName);
         sb.Append(".");
         sb.Append(method.Name);
         sb.Append("(");
         bool isFirst = true;
         foreach(ParameterInfo p in method.GetParameters())
         {
            if(!isFirst) sb.Append(", ");
            sb.Append(p.ParameterType.Name);
            sb.Append(" ");
            sb.Append(p.Name);
         }
         sb.Append(")");

         propertyValue = sb.ToString();
      }
   }
#endif
}
