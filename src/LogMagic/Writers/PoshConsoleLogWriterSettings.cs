namespace LogMagic.Writers
{
   /// <summary>
   /// Configuration settings for <see cref="PoshConsoleLogWriter"/>
   /// </summary>
   public class PoshConsoleLogWriterSettings
   {
      /// <summary>
      /// When set to true (default) abbreviates class names in log output. For example
      /// PoshConsoleLogReceiver becomes PCLR. False by default.
      /// </summary>
      public bool AbbreviateClassNames { get; set; }
   }
}
