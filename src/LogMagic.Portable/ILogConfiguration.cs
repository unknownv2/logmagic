using System.Collections.Generic;

namespace LogMagic
{
   public interface ILogConfiguration
   {
      ILogConfiguration ClearWriters();

      ILogConfiguration AddWriter(ILogWriter writer);

      ILogConfiguration AddEnricher(IEnricher enricher);

      IEnumerable<ILogWriter> Writers { get; }

      IEnumerable<IEnricher> Enrichers { get; }
   }
}
