using LogMagic;
using LogMagic.PerfCounters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LogMagic.Configuration
{
   class LogConfiguration : ILogConfiguration, IWriterConfiguration, IEnricherConfiguration, IFilterConfiguration, IPerformanceCounterConfiguration
   {
      private readonly List<ILogWriter> _writers = new List<ILogWriter>();
      private readonly List<IEnricher> _enrichers = new List<IEnricher>();
      private readonly Dictionary<ILogWriter, List<IFilter>> _activeFilters = new Dictionary<ILogWriter, List<IFilter>>();
      private readonly List<IPerformanceCounter> _perfCounters = new List<IPerformanceCounter>();

      public IEnumerable<IEnricher> Enrichers => _enrichers;

      public IEnumerable<ILogWriter> Writers => _writers;

      public IReadOnlyCollection<IPerformanceCounter> PerformanceCounters => _perfCounters;

      public ILogConfiguration ClearFilters()
      {
         _activeFilters.Clear();

         return this;
      }

      public ILogConfiguration ClearWriters()
      {
         _writers.Clear();

         return this;
      }

      public ILogConfiguration ClearEnrichers()
      {
         _enrichers.Clear();

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

      public ILogConfiguration Custom(IFilter filter)
      {
         ILogWriter writer = _writers.LastOrDefault();
         if (writer == null) return this;

         if(!_activeFilters.TryGetValue(writer, out List<IFilter> filters))
         {
            filters = new List<IFilter>();
            _activeFilters[writer] = filters;
         }

         filters.Add(filter);
         return this;
      }

      public IReadOnlyCollection<IFilter> GetFilters(ILogWriter writer)
      {
         if (!_activeFilters.TryGetValue(writer, out List<IFilter> filters)) return null;
         return filters;
      }

      public ILogConfiguration Custom(IPerformanceCounter performanceCounter)
      {
         if (performanceCounter != null)
         {
            _perfCounters.Add(performanceCounter);
         }

         return this;
      }

      public ILogConfiguration WithSamplingInterval(TimeSpan samplingInterval)
      {
         return this;
      }

      public void Shutdown()
      {
         foreach(ILogWriter writer in _writers)
         {
            writer.Dispose();
         }
      }

      public IFilterConfiguration When => this;

      public IPerformanceCounterConfiguration CollectPerformanceCounters => this;
   }
}
