using System;
using LogMagic.Seq;

namespace LogMagic
{
   public static class ConfigurationExtensions
   {
      public static ILogConfiguration Seq(this IWriterConfiguration configuration, Uri serverAddress)
      {
         return configuration.Custom(new SeqWriter(serverAddress));
      }
   }
}
