#if !NET45
using System;
using System.Collections.Concurrent;
using LogMagic.Enrichers;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace LogMagic
{
   /// <summary>
   /// Logging context
   /// </summary>
   public class LogContext
   {
      private readonly AsyncLocal<ConcurrentDictionary<string, IEnricher>> Data =
         new AsyncLocal<ConcurrentDictionary<string, IEnricher>>();

      /// <summary>
      /// Push
      /// </summary>
      public IDisposable Push(IEnumerable<KeyValuePair<string, string>> properties)
      {
         ConcurrentDictionary<string, IEnricher> stack = GetOrCreateEnricherStack();
         var bookmark = new StackBookmark(this, Clone(stack));

         if (properties != null)
         {
            foreach (KeyValuePair<string, string> pair in properties)
            {
               stack[pair.Key] = new ConstantEnricher(pair);
            }
         }

         Enrichers = stack;

         return bookmark;
      }

      private ConcurrentDictionary<string, IEnricher> Clone(ConcurrentDictionary<string, IEnricher> stack)
      {
         return new ConcurrentDictionary<string, IEnricher>(stack);
      }

      private ConcurrentDictionary<string, IEnricher> GetOrCreateEnricherStack()
      {
         ConcurrentDictionary<string, IEnricher> enrichers = Enrichers;

         if (enrichers == null)
         {
            enrichers = new ConcurrentDictionary<string, IEnricher>();
            Enrichers = enrichers;
         }

         return enrichers;
      }

      /// <summary>
      /// List of active enrichers
      /// </summary>
      public ConcurrentDictionary<string, IEnricher> Enrichers
      {
         get => Data.Value;
         set => Data.Value = value;
      }

      /// <summary>
      /// Gets context property by name
      /// </summary>
      public string GetValueByName(string name)
      {
         ConcurrentDictionary<string, IEnricher> enrichers = Enrichers;

         if (enrichers == null) return null;

         ConstantEnricher enricher = enrichers
            .Values
            .Cast<ConstantEnricher>()
            .FirstOrDefault(e => e.Name == name);

         if (enricher == null) return null;

         return enricher.Value;
      }

      /// <summary>
      /// Gets all context values
      /// </summary>
      public Dictionary<string, string> GetAllValues()
      {
         ConcurrentDictionary<string, IEnricher> enrichers = Enrichers;

         if (enrichers == null) return new Dictionary<string, string>();

         return enrichers
            .Values
            .Cast<ConstantEnricher>()
            .ToDictionary(c => c.Name, c => c.Value);
      }

      sealed class StackBookmark : IDisposable
      {
         private readonly LogContext _context;
         private readonly ConcurrentDictionary<string, IEnricher> _enrichers;

         public StackBookmark(LogContext context, ConcurrentDictionary<string, IEnricher> bookmark)
         {
            _context = context;
            _enrichers = bookmark;
         }

         public void Dispose()
         {
            _context.Enrichers = _enrichers;
         }
      }
   }
}
#endif