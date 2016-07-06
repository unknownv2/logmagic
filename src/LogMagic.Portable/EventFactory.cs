using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogMagic.TypeFormatters;

namespace LogMagic
{
   static class EventFactory
   {
      public static LogEvent CreateEvent(string sourceName, LogSeverity severity, string format, object[] parameters)
      {
         var e = new LogEvent(severity, sourceName, DateTime.UtcNow);

         //add error
         Exception error = ExtractError(parameters);
         if (error != null) e.AddProperty("error", error.ToString());

         //enrich
         foreach(IEnricher enricher in L.Config.Enrichers)
         {
            string pn, pv;
            enricher.Enrich(e, out pn, out pv);
            e.AddProperty(pn, pv);
         }

         //message
         e.Message = FormatMessage(e, format, parameters);

         return e;
      }

      private static Exception ExtractError(object[] parameters)
      {
         if (parameters != null && parameters.Length > 0)
         {
            Exception error = parameters[parameters.Length - 1] as Exception;
            if (error != null)
            {
               Array.Resize(ref parameters, parameters.Length - 1);
               return error;
            }
         }

         return null;
      }

      private static string FormatMessage(LogEvent e, string format, object[] parameters)
      {
         string message = parameters == null ? format : string.Format(format, parameters.Select(FormatterEntry.FormatParameter).ToArray());
         return message;
      }
   }
}
