using System;
using System.Runtime.CompilerServices;
using LogMagic.Tokenisation;
using LogMagic.Enrichers;
using System.Collections.Generic;

namespace LogMagic
{
   class EventFactory
   {
#if !NET45
      private readonly LogContext _context;

      public EventFactory(LogContext context)
      {
         _context = context;
      }
#endif

      /// <summary>
      /// Creates a rich logging event
      /// </summary>
      /// <param name="sourceName"></param>
      /// <param name="eventType"></param>
      /// <param name="format"></param>
      /// <param name="parameters"></param>
      /// <returns></returns>
      [MethodImpl(MethodImplOptions.NoInlining)]
      public LogEvent CreateEvent(string sourceName, EventType eventType, string format, object[] parameters)
      {
         var e = new LogEvent(sourceName, DateTime.UtcNow) { EventType = eventType };

         //add error
         Exception error = ExtractError(parameters);
         if (error != null) e.AddProperty(KnownProperty.Error, error);

         //enrich
         Enrich(e, L.Config.Enrichers);
#if NETSTANDARD || NET46
         Enrich(e, _context.Enrichers?.Values);
#endif

         //message
         FormattedString fs = FormattedString.Parse(format, parameters);
         e.Message = fs;
         e.FormattedMessage = fs.ToString();
         foreach(KeyValuePair<string, object> namedParam in fs.NamedParameters)
         {
            e.AddProperty(namedParam.Key, namedParam.Value);
         }

         return e;
      }

      private void Enrich(LogEvent e, IEnumerable<IEnricher> enrichers)
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
