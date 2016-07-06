using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogMagic.Configuration
{
   class LogConfiguration : ILogConfiguration
   {
      private readonly List<ILogWriter> _writers = new List<ILogWriter>();
      private readonly List<IEnricher> _enrichers = new List<IEnricher>();

      public IEnumerable<IEnricher> Enrichers => _enrichers;

      public IEnumerable<ILogWriter> Writers => _writers;

      public ILogConfiguration AddWriter(ILogWriter writer)
      {
         _writers.Add(writer);

         return this;
      }

      public ILogConfiguration Enrich(IEnricher enricher)
      {
         _enrichers.Add(enricher);

         return this;
      }

      public ILogConfiguration AddEnricher(IEnricher enricher)
      {
         _enrichers.Add(enricher);

         return this;
      }

      public ILogConfiguration ClearWriters()
      {
         _writers.Clear();

         return this;
      }
   }
}
