using LogMagic.Tokenisation;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogMagic
{
   internal class FormatStringBuilder
   {
      private readonly List<MessageToken> _tokens = new List<MessageToken>();

      public FormatStringBuilder AddText(string text)
      {
         if (text == null)
         {
            throw new ArgumentNullException(nameof(text));
         }

         _tokens.Add(new MessageToken(TokenType.String, null, null));

         return this;
      }

      /// <summary>
      /// Adds time token with optional formatting
      /// </summary>
      /// <param name="format">Native C# format string for <see cref="DateTime"/></param>
      /// <returns></returns>
      public FormatStringBuilder AddTime(string format = null)
      {
         _tokens.Add(new MessageToken(TokenType.Time, null, format));
         return this;
      }


      public FormattedString Build()
      {
         throw new NotImplementedException();
      }


   }
}
