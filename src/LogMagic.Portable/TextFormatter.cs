using System;
using LogMagic.Model;

namespace LogMagic
{
   static class StandardFormatter
   {
      const string BlockSeparator = "|";

      /// <summary>
      /// Formats a log chunk into a string
      /// </summary>
      /// <param name="chunk"></param>
      /// <returns></returns>
      public static string Format(LogChunk chunk)
      {
         string nodeId = string.IsNullOrEmpty(L.NodeId) ? string.Empty : (L.NodeId + "|");
         string errorString = chunk.Error == null ? string.Empty : (Environment.NewLine + chunk.Error);
         string line = $"{chunk.EventTime.ToString("H:mm:ss,fff")}|{nodeId}{chunk.SourceName}|{chunk.ThreadName}|{chunk.Message}{errorString}{Environment.NewLine}";
         return line;
      }

      public static string Format(LogEvent chunk)
      {
         string nodeId = string.IsNullOrEmpty(L.NodeId) ? string.Empty : (L.NodeId + "|");
         string errorString = chunk.Error == null ? string.Empty : (Environment.NewLine + chunk.Error);
         string line = $"{chunk.EventTime.ToString("H:mm:ss,fff")}|{nodeId}{chunk.SourceName}|{chunk.ThreadName}|{chunk.Message}{errorString}{Environment.NewLine}";
         return line;
      }
   }
}
