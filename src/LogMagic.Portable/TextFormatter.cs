using System;
using System.Text;

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
      public static string Format(LogEvent e, bool appendEnrichedProperties)
      {
         var b = new StringBuilder();
         b.Append(e.EventTime.ToString("H:mm:ss,fff"));
         b.Append(BlockSeparator);
         b.Append(GetLogSeverity(e.Severity));
         b.Append(BlockSeparator);
         b.Append(e.SourceName);
         b.Append(BlockSeparator);
         b.Append(e.FormattedMessage);

         object error = e.GetProperty(LogEvent.ErrorPropertyName);
         if(error != null)
         {
            b.Append(Environment.NewLine);
            b.Append(error.ToString());
         }

         if(appendEnrichedProperties && e.Properties != null)
         {
            b.Append(Environment.NewLine);
            bool firstProp = true;
            foreach(var p in e.Properties)
            {
               if (!firstProp)
               {
                  b.Append("; ");
               }
               else
               {
                  firstProp = false;
               }

               b.Append(p.Key);
               b.Append("='");
               b.Append(p.Value?.ToString());
               b.Append("'");
            }
         }

         return b.ToString();
      }

      private static string GetLogSeverity(LogSeverity s)
      {
         switch (s)
         {
            case LogSeverity.Debug:
               return "DEBUG";
            case LogSeverity.Error:
               return "ERROR";
            case LogSeverity.Info:
               return "INFO ";
            case LogSeverity.Warning:
               return "WARN ";
            default:
               return string.Empty;
         }
      }
   }
}
