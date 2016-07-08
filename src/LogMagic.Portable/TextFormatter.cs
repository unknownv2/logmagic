using System;

namespace LogMagic
{
   /// <summary>
   /// Log formatting utility methods
   /// </summary>
   public static class TextFormatter
   {
      const string BlockSeparator = "|";

      /// <summary>
      /// Formats log event for text representation, not including any properties. Error is included though.
      /// </summary>
      public static string Format(LogEvent chunk)
      {
         object error = chunk.GetProperty(LogEvent.ErrorPropertyName);

         string errorString = error == null ? string.Empty : (Environment.NewLine + error);
         string line = $"{chunk.EventTime.ToString("H:mm:ss,fff")}{BlockSeparator}{chunk.SourceName}{BlockSeparator}{chunk.Message}{errorString}{Environment.NewLine}";
         return line;
      }
   }
}
