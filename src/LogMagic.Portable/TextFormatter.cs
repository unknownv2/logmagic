using System;

namespace LogMagic
{
   public static class TextFormatter
   {
      const string BlockSeparator = "|";

      public static string Format(LogEvent chunk)
      {
         string nodeId = string.IsNullOrEmpty(L.NodeId) ? string.Empty : (L.NodeId + "|");
         string errorString = chunk.Error == null ? string.Empty : (Environment.NewLine + chunk.Error);
         string line = $"{chunk.EventTime.ToString("H:mm:ss,fff")}|{nodeId}{chunk.SourceName}|{chunk.Message}{errorString}{Environment.NewLine}";
         return line;
      }
   }
}
