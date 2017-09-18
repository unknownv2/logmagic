using System.Text;
using LogMagic.Tokenisation;

namespace LogMagic
{
   /// <summary>
   /// Log formatting utility methods
   /// </summary>
   public static class TextFormatter
   {
      /// <summary>
      /// Default format string used to format log text
      /// </summary>
      public static readonly FormattedString DefaultFormat = FormattedString.Parse("{time:H:mm:ss,fff}|{level,-7}|{source}|{message}{error}", null);

      internal const string Time = "time";
      internal const string Severity = "level";
      internal const string Source = "source";
      internal const string Message = "message";
      internal const string Error = "error";
      internal const string NewLine = "br";

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
                        string sev = e.ErrorException == null ? "I" : "E";
                        if (token.Format != null) sev = string.Format(token.NativeFormat, sev);
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
   }
}
