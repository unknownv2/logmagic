using System.Text;
using LogMagic.Tokenisation;

namespace LogMagic
{
   /// <summary>
   /// Log formatting utility methods
   /// </summary>
   public static class TextFormatter
   {
      const string BlockSeparator = "|";
      static readonly FormattedString DefaultFormat = FormattedString.Parse("{time:H:mm:ss,fff}|{level:LLL}|{source}|{message}{error}", null);

      const string Time = "time";
      const string Severity = "level";
      const string Source = "source";
      const string Message = "message";
      const string Error = "error";
      const string NewLine = "br";

      /// <summary>
      /// Formats log event for text representation, not including any properties. Error is included though.
      /// </summary>
      public static string Format(LogEvent e, FormattedString format = null)
      {
         if (format == null) format = DefaultFormat;

         var b = new StringBuilder();

         foreach(Token token in format.Tokens)
         {
            switch(token.Type)
            {
               case TokenType.String:
                  b.Append(token.Value);
                  break;
               case TokenType.Parameter:
                  switch(token.Name)
                  {
                     case Time:
                        b.Append(e.EventTime.ToString(token.Format));
                        break;
                     case Severity:
                        string sev = e.Severity.ToString().ToUpper();
                        if(token.Format != null && token.Format.Length < sev.Length)
                        {
                           sev = sev.Substring(0, token.Format.Length);
                        }

                        b.Append(sev);
                        break;
                     case Source:
                        b.Append(e.SourceName);
                        break;
                     case Message:
                        b.Append(e.FormattedMessage);
                        break;
                     case Error:
                        if (e.ErrorException != null)
                        {
                           b.AppendLine();
                           b.Append(e.ErrorException.ToString());
                        }
                        break;
                     case NewLine:
                        b.AppendLine();
                        break;
                     default:
                        if(e.Properties != null)
                        {
                           object value;
                           if(e.Properties.TryGetValue(token.Name, out value))
                           {
                              string custom = format.Format(token, value);
                              b.Append(custom);
                           }
                        }
                        break;
                  }
                  break;
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
