using System;

namespace LogMagic
{
   public static class TextFormatter
   {
      const string BlockSeparator = "|";

      public static string Format(LogEvent chunk)
      {
         string error = chunk.GetProperty(LogEvent.ErrorPropertyName);

         string errorString = error == null ? string.Empty : (Environment.NewLine + error);
         string line = $"{chunk.EventTime.ToString("H:mm:ss,fff")}|{chunk.SourceName}|{chunk.Message}{errorString}{Environment.NewLine}";
         return line;
      }
   }
}
