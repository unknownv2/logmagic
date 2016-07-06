using LogMagic.Writers;

namespace LogMagic
{
   public static class WritersExtensions
   {
      public static ILogConfiguration Console(this IWriterConfiguration configuration)
      {
         return configuration.Custom(new ConsoleLogWriter());
      }

      public static ILogConfiguration PoshConsole(this IWriterConfiguration configuration)
      {
         return configuration.Custom(new PoshConsoleLogWriter());
      }

      public static ILogConfiguration Trace(this IWriterConfiguration configuration)
      {
         return configuration.Custom(new TraceLogWriter());
      }

      public static ILogConfiguration File(this IWriterConfiguration configuration, string fileName)
      {
         return configuration.Custom(new FileLogWriter(fileName));
      }
   }
}
