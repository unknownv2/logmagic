using LogMagic.Writers;

namespace LogMagic
{
   public static class WriterExtensions
   {
      public static ILogConfiguration WriteToConsole(this ILogConfiguration configuration)
      {
         return configuration.AddWriter(new ConsoleLogWriter());
      }

      public static ILogConfiguration WriteToColoredConsole(this ILogConfiguration configuration)
      {
         return configuration.AddWriter(new PoshConsoleLogWriter());
      }

      public static ILogConfiguration WriteToTrace(this ILogConfiguration configuration)
      {
         return configuration.AddWriter(new TraceLogWriter());
      }

      public static ILogConfiguration WriteToFile(this ILogConfiguration configuration, string fileName)
      {
         return configuration.AddWriter(new FileLogWriter(fileName));
      }
   }
}
