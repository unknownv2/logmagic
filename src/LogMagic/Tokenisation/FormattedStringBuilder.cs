using LogMagic.Tokenisation;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogMagic.Tokenisation
{
   public class FormattedStringBuilder
   {
      private readonly StringBuilder _string = new StringBuilder();

      public FormattedStringBuilder AddText(string text)
      {
         if (text == null)
         {
            throw new ArgumentNullException(nameof(text));
         }

         _string.Append(text);

         return this;
      }

      /// <summary>
      /// Adds time token with optional formatting
      /// </summary>
      /// <param name="format">Native C# format string for <see cref="DateTime"/></param>
      /// <returns></returns>
      public FormattedStringBuilder AddTime(string format = null)
      {
         _string.Append(FormattedString.ParamBegin);
         _string.Append(TextFormatter.Time);

         if(format != null)
         {
            _string.Append(FormattedString.FormatSeparator);
            _string.Append(format);
         }

         _string.Append(FormattedString.ParamEnd);
         
         return this;
      }

      public FormattedStringBuilder AddLevel(int pad = 0)
      {
         _string.Append(FormattedString.ParamBegin);
         _string.Append(TextFormatter.Severity);
         
         if(pad != 0)
         {
            _string.AppendFormat(",{0}", pad);
         }

         _string.Append(FormattedString.ParamEnd);

         return this;
      }

      public string Build()
      {
         return _string.ToString();
      }

   }
}
