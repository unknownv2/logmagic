using System;
using System.Runtime.CompilerServices;
using LogMagic.Tokenisation;
using LogMagic.Enrichers;
using System.Collections.Generic;

namespace LogMagic
{
   static class EventFactory
   {
      [MethodImpl(MethodImplOptions.NoInlining)]
      public static LogEvent CreateEvent(string sourceName, EventType eventType, string format, object[] parameters)
      {
         var e = new LogEvent(sourceName, DateTime.UtcNow) { EventType = eventType };

         //add error
         Exception error = ExtractError(parameters);
         if (error != null) e.AddProperty(KnownProperty.Error, error);

         //enrich
         Enrich(e, L.Config.Enrichers);
#if NETSTANDARD || NET46
         Enrich(e, LogContext.Enrichers?.Values);
#endif

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

      private static void Enrich(LogEvent e, IEnumerable<IEnricher> enrichers)
      {
         if (enrichers == null) return;

         foreach (IEnricher enricher in enrichers)
         {
            string pn;
            object pv;
            enricher.Enrich(e, out pn, out pv);
            if (pn != null)
            {
               e.AddProperty(pn, pv);
            }
         }
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
