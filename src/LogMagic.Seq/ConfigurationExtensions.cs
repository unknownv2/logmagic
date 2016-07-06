using System;
using LogMagic.Seq;

namespace LogMagic
{
   public static class ConfigurationExtensions
   {
      public static ILogConfiguration WriteToSeq(this ILogConfiguration configuration, Uri serverAddress)
      {
         return configuration.AddWriter(new SeqWriter(serverAddress));
      }
   }
}
