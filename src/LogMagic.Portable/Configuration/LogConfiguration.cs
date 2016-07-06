using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogMagic.Configuration
{
   class LogConfiguration : ILogConfiguration, IWriterConfiguration, IEnricherConfiguration
   {
      private readonly List<ILogWriter> _writers = new List<ILogWriter>();
      private readonly List<IEnricher> _enrichers = new List<IEnricher>();

      public IEnumerable<IEnricher> Enrichers => _enrichers;

      public IEnumerable<ILogWriter> Writers => _writers;

      public ILogConfiguration ClearWriters()
      {
         _writers.Clear();

         return this;
      }

      public ILogConfiguration Custom(ILogWriter writer)
      {
         _writers.Add(writer);

         return this;
      }

      public IWriterConfiguration WriteTo => this;


      public ILogConfiguration Enrich(IEnricher enricher)
      {
         _enrichers.Add(enricher);

         return this;
      }

      public ILogConfiguration Custom(IEnricher enricher)
      {
         _enrichers.Add(enricher);

         return this;
      }

      public IEnricherConfiguration EnrichWith => this;
   }
}
