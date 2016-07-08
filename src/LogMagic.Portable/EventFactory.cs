using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using LogMagic.TypeFormatters;

namespace LogMagic
{
   static class EventFactory
   {
      private static readonly char[] ParamTrims = new char[] { '{', '}' };

      private struct TokenTag
      {
         public string Value;
         public bool IsParameter;
         public string Formatted;

         public TokenTag(string value, bool isParameter)
         {
            Value = value;
            IsParameter = isParameter;
            Formatted = null;
         }
      }

      [MethodImpl(MethodImplOptions.NoInlining)]
      public static LogEvent CreateEvent(string sourceName, LogSeverity severity, string format, object[] parameters)
      {
         var e = new LogEvent(severity, sourceName, DateTime.UtcNow);

         //add error
         Exception error = ExtractError(parameters);
         if (error != null) e.AddProperty("error", error.ToString());

         //enrich
         foreach(IEnricher enricher in L.Config.Enrichers)
         {
            string pn;
            object pv;
            enricher.Enrich(e, out pn, out pv);
            e.AddProperty(pn, pv);
         }

         //message
         e.Message = FormatMessage(e, format, parameters);

         return e;
      }

      private static Exception ExtractError(object[] parameters)
      {
         if (parameters != null && parameters.Length > 0)
         {
            Exception error = parameters[parameters.Length - 1] as Exception;
            if (error != null)
            {
               Array.Resize(ref parameters, parameters.Length - 1);
               return error;
            }
         }

         return null;
      }

      private static string FormatMessage(LogEvent e, string format, object[] parameters)
      {
         if (parameters == null || parameters.Length == 0) return format;

         List<TokenTag> tokens = Tokenise(format);
         var r = new StringBuilder();

         int pi = 0;
         for(int i = 0; i < tokens.Count; i++)
         {
            TokenTag tag = tokens[i];
            if (tag.IsParameter)
            {
               int pos;
               string nativeFormat;
               string paramName;
               ParseToken(tag.Value, out paramName, out pos, out nativeFormat);

               if (pos == -1) pos = pi;
               if(pos < parameters.Length)
               {
                  object inputValue = parameters[pos];
                  string outputValue = FormatterEntry.FormatParameter(inputValue);
                  if (outputValue == null) outputValue = nativeFormat == null 
                        ? inputValue?.ToString() 
                        : string.Format(nativeFormat, inputValue);
                  r.Append(outputValue);
                  e.AddProperty(paramName, inputValue);
               }
               else
               {
                  r.Append(tag.Value);
               }

               pi++;
            }
            else
            {
               r.Append(tag.Value);
            }
         }

         return r.ToString();
      }

      private static void ParseToken(string value, out string paramName, out int position, out string nativeFormat)
      {
         value = value.Trim(ParamTrims);

         int idx = value.IndexOf(":");
         string format = idx == -1 ? null : value.Substring(idx + 1);
         string pos = idx == -1 ? value : value.Substring(0, idx);

         if (!int.TryParse(pos, out position))
         {
            position = -1;
            paramName = pos;
         }
         else
         {
            paramName = null;
         }
         nativeFormat = format == null
            ? null
            : $"{{0:{format}}}";
      }

      private static List<TokenTag> Tokenise(string format)
      {
         string token = string.Empty;
         bool isParameter = false;
         var tokens = new List<TokenTag>();

         foreach(char ch in format)
         {
            switch(ch)
            {
               case '{':
                  if(token != string.Empty)
                  {
                     tokens.Add(new TokenTag(token, isParameter));
                  }
                  isParameter = true;
                  token = string.Empty;
                  token += ch;
                  break;
               case '}':
                  if(token != string.Empty)
                  {
                     token += ch;
                     tokens.Add(new TokenTag(token, isParameter));
                  }
                  token = string.Empty;
                  isParameter = false;
                  break;
               default:
                  token += ch;
                  break;
            }
         }

         if (token != string.Empty) tokens.Add(new TokenTag(token, false));

         return tokens;
      }
   }
}
