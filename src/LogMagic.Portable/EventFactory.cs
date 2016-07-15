using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using LogMagic.Tokenisation;
using LogMagic.TypeFormatters;

namespace LogMagic
{
   static class EventFactory
   {
      [MethodImpl(MethodImplOptions.NoInlining)]
      public static LogEvent CreateEvent(string sourceName, LogSeverity severity, string format, object[] parameters)
      {
         var e = new LogEvent(severity, sourceName, DateTime.UtcNow);

         //add error
         Exception error = ExtractError(parameters);
         if (error != null) e.AddProperty(LogEvent.ErrorPropertyName, error);

         //enrich
         foreach(IEnricher enricher in L.Config.Enrichers)
         {
            string pn;
            object pv;
            enricher.Enrich(e, out pn, out pv);
            e.AddProperty(pn, pv);
         }

         //message
         FormattedString fs = FormattedString.Parse(format, parameters);
         e.Message = fs;
         e.FormattedMessage = fs.ToString();
         foreach(var namedParam in fs.NamedParameters)
         {
            e.AddProperty(namedParam.Key, namedParam.Value);
         }

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
   }
}
