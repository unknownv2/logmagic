using System;

namespace LogMagic.Formatters
{
   /// <summary>
   /// Default formatter
   /// </summary>
   public class StandardFormatter : IFormatter
   {
      /// <summary>
      /// Formats a log chunk into a string
      /// </summary>
      /// <param name="chunk"></param>
      /// <returns></returns>
      public string Format(LogChunk chunk)
      {
         string errorString = chunk.Error == null ? string.Empty : (Environment.NewLine + chunk.Error);
         string line = $"{chunk.EventTime.ToString("H:mm:ss,fff")}|{chunk.SourceName}|{chunk.ThreadName}|{chunk.Message}{errorString}{Environment.NewLine}";
         return line;
      }
   }
}
