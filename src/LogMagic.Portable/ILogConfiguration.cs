using System.Collections.Generic;

namespace LogMagic
{
   public interface ILogConfiguration
   {
      ILogConfiguration ClearWriters();

      IEnumerable<IEnricher> Enrichers { get; }

      IEnumerable<ILogWriter> Writers { get; }

      IEnricherConfiguration EnrichWith { get; }

      IWriterConfiguration WriteTo { get; }
   }
}
