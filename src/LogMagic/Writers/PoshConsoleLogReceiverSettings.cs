namespace LogMagic.Writers
{
   /// <summary>
   /// Configuration settings for <see cref="PoshConsoleLogWriter"/>
   /// </summary>
   public class PoshConsoleLogReceiverSettings
   {
      /// <summary>
      /// When set to true (default) abbreviates class names in log output. For example
      /// PoshConsoleLogReceiver becomes PCLR.
      /// </summary>
      public bool AbbreviateClassNames { get; set; }
   }
}
