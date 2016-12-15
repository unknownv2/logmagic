using System;
using LogMagic.Seq;

namespace LogMagic
{
   public static class ConfigurationExtensions
   {
      public static ILogConfiguration Seq(this IWriterConfiguration configuration, Uri serverAddress)
      {
         return configuration.Custom(new SeqWriter(serverAddress, null));
      }

      public static ILogConfiguration Seq(this IWriterConfiguration configuration, Uri serverAddress, string apiKey)
      {
         return configuration.Custom(new SeqWriter(serverAddress, apiKey));
      }
   }
}
