using System.Collections.Generic;
using System.Text;
using LogMagic.TypeFormatters;

namespace LogMagic.Tokenisation
{
   /// <summary>
   /// Represents a formatted log string
   /// </summary>
   public class FormattedString
   {
      private static readonly char[] ParamTrims = new char[] { '{', '}' };
      private readonly string _format;
      private readonly object[] _parameters;
      private Token[] _tokens;
      private int _namedPos;

      private FormattedString(string format, object[] parameters)
      {
         _format = format;
         _parameters = parameters;
      }

      /// <summary>
      /// Creates an instance by specifying format string and parameters
      /// </summary>
      public static FormattedString Parse(string format, object[] parameters)
      {
         var fmt = new FormattedString(format, parameters);
         fmt.Parse();
         return fmt;
      }

      /// <summary>
      /// Returns all the tokens
      /// </summary>
      public IReadOnlyCollection<Token> Tokens => _tokens;

      /// <summary>
      /// Returns named parameters
      /// </summary>
      public IReadOnlyDictionary<string, object> NamedParameters
      {
         get
         {
            var r = new Dictionary<string, object>();
            if (_parameters != null)
            {
               foreach (Token token in _tokens)
               {
                  if (token.Type == TokenType.Parameter && token.Name != null && token.Position < _parameters.Length)
                  {
                     r[token.Name] = _parameters[token.Position];
                  }
               }
            }
            return r;
         }
      }

      /// <summary>
      /// Formats the token as a string
      /// </summary>
      public string Format(Token token, object value = null)
      {
         switch(token.Type)
         {
            case TokenType.String:
               return token.Value;
            case TokenType.Parameter:
               if (value == null && _parameters == null) return string.Empty;

               if (value == null && token.Position < _parameters.Length)
               {
                  value = _parameters[token.Position];
                  value = FormatterEntry.FormatParameter(value);
               }

               if(token.NativeFormat != null)
               {
                  return string.Format(token.NativeFormat, value);
               }
               else
               {
                  return FormatterEntry.FormatParameter(value).ToString();
               }
         }

         return string.Empty;
      }

      /// <summary>
      /// Converts to full string representation formatting all the parameters
      /// </summary>
      public override string ToString()
      {
         if (_format == null) return string.Empty;

         var sb = new StringBuilder();

         foreach(Token token in _tokens)
         {
            sb.Append(Format(token));
         }

         return sb.ToString();
      }

      private void Parse()
      {
         if (_format == null) return;

         _tokens = Tokenise(_format).ToArray();
      }

      private List<Token> Tokenise(string format)
      {
         string token = string.Empty;
         bool isParameter = false;
         var tokens = new List<Token>();

         foreach (char ch in format)
         {
            switch (ch)
            {
               case '{':
                  if (token != string.Empty)
                  {
                     tokens.Add(CreateToken(isParameter, token));
                  }
                  isParameter = true;
                  token = string.Empty;
                  token += ch;
                  break;
               case '}':
                  if (token != string.Empty)
                  {
                     token += ch;
                     tokens.Add(CreateToken(isParameter, token));
                  }
                  token = string.Empty;
                  isParameter = false;
                  break;
               default:
                  token += ch;
                  break;
            }
         }

         if (token != string.Empty) tokens.Add(CreateToken(false, token));

         return tokens;
      }

      private Token CreateToken(bool isParameter, string value)
      {
         TokenType type = isParameter ? TokenType.Parameter : TokenType.String;
         string name = null;
         int position = 0;
         string nativeFormat = null;
         string format = null;

         if(isParameter)
         {
            value = value.Trim(ParamTrims);

            int idx = value.IndexOf(":");
            format = idx == -1 ? null : value.Substring(idx + 1);
            string pos = idx == -1 ? value : value.Substring(0, idx);

            int p;
            if (!int.TryParse(pos, out p))
            {
               name = pos;
               position = _namedPos;
            }
            else
            {
               position = p;
            }

            nativeFormat = format == null
               ? null
               : $"{{0:{format}}}";
            _namedPos++;
         }

         return new Token(type, value, name, position, nativeFormat, format);
      }
   }
}
