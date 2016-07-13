using LogMagic.Writers;

namespace LogMagic
{
   /// <summary>
   /// Extensions methods to help initialise configuration
   /// </summary>
   public static class WritersExtensions
   {
      /// <summary>
      /// Writes to system console
      /// </summary>
      public static ILogConfiguration Console(this IWriterConfiguration configuration)
      {
         return configuration.Custom(new ConsoleLogWriter());
      }

      /// <summary>
      /// Writes to posh system console i.e. with nice colours
      /// </summary>
      public static ILogConfiguration PoshConsole(this IWriterConfiguration configuration)
      {
         return configuration.Custom(new PoshConsoleLogWriter());
      }

      /// <summary>
      /// Writes to .NET trace
      /// </summary>
      public static ILogConfiguration Trace(this IWriterConfiguration configuration)
      {
         return configuration.Custom(new TraceLogWriter());
      }

      /// <summary>
      /// Writes to file on disk
      /// </summary>
      public static ILogConfiguration File(this IWriterConfiguration configuration, string fileName)
      {
         return configuration.Custom(new FileLogWriter(fileName));
      }
   }
}
