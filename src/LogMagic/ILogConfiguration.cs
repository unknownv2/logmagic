using System.Collections.Generic;

namespace LogMagic
{
   /// <summary>
   /// Entry point to logging configuration
   /// </summary>
   public interface ILogConfiguration
   {
      /// <summary>
      /// Removes all configured writers
      /// </summary>
      /// <returns></returns>
      ILogConfiguration ClearWriters();

      /// <summary>
      /// Gets the list of configured enrichers
      /// </summary>
      IEnumerable<IEnricher> Enrichers { get; }

      /// <summary>
      /// Gets the list of configured writers
      /// </summary>
      IEnumerable<ILogWriter> Writers { get; }

      /// <summary>
      /// Entry point to enrichers configuration
      /// </summary>
      IEnricherConfiguration EnrichWith { get; }

      /// <summary>
      /// Entry point to writers configuration
      /// </summary>
      IWriterConfiguration WriteTo { get; }

      /// <summary>
      /// Entry point to filters configuration
      /// </summary>
      IFilterConfiguration FilterBy { get; }
   }
}
