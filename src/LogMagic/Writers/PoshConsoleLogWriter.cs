using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogMagic.Tokenisation;

namespace LogMagic.Writers
{
   /// <summary>
   /// Posh colorful console, you should use this if you want to look cool.
   /// </summary>
   class PoshConsoleLogWriter : ILogWriter
   {
      private static readonly ConcurrentDictionary<string, string> SourceNameToShortName = new ConcurrentDictionary<string, string>();
      private static object ColourLock = new object();

      private const ConsoleColor SeparatorColour = ConsoleColor.DarkGray,
                                 MessageColour = ConsoleColor.White,
                                 ParameterColour = ConsoleColor.Green,
                                 ExceptionHeadlineColour = ConsoleColor.Red,
                                 ExceptionStackTraceColour = ConsoleColor.Gray;
      private const string Pipe = "|";

      /// <summary>
      /// Receiver's settings
      /// </summary>
      public PoshConsoleLogWriterSettings Settings { get; private set; }

      private int _classNamePadding = 10;

      /// <summary>
      /// Constructs and instance of this class
      /// </summary>
      public PoshConsoleLogWriter()
      {
         Settings = new PoshConsoleLogWriterSettings
         {
            AbbreviateClassNames = false
         };

         Console.BackgroundColor = ConsoleColor.Black;
      }

      /// <summary>
      /// Sends the chunk to posh console!
      /// </summary>
      private void Write(LogEvent e)
      {
         //timestamp
         Cg.Write(e.EventTime.ToString("HH"), ConsoleColor.Green);
         Cg.Write(":", SeparatorColour);
         Cg.Write(e.EventTime.ToString("mm"), ConsoleColor.Green);
         Cg.Write(":", SeparatorColour);
         Cg.Write(e.EventTime.ToString("ss"), ConsoleColor.Green);
         Cg.Write(",", SeparatorColour);
         Cg.Write(e.EventTime.ToString("fff"), ConsoleColor.DarkGreen);

         //level
         Cg.Write(Pipe, SeparatorColour);
         LogSeverity(e.Severity);

         //source
         //Cg.Write(Pipe, SeparatorColour);
         //Cg.Write(Abbreviate(e.SourceName), ConsoleColor.Gray);

         //message
         Cg.Write(Pipe, SeparatorColour);
         foreach(Token token in e.Message.Tokens)
         {
            switch(token.Type)
            {
               case TokenType.String:
                  Cg.Write(e.Message.Format(token), MessageColour);
                  break;
               case TokenType.Parameter:
                  Cg.Write(e.Message.Format(token), ParameterColour);
                  break;
            }
         }

         //error
         Exception ex = e.ErrorException;
         if(ex != null)
         {
            Console.WriteLine();
            Cg.Write($"{ex.GetType()}: {ex.Message}", ExceptionHeadlineColour);

            Console.WriteLine();
            Cg.Write(ex.StackTrace, ExceptionStackTraceColour);
         }

         //extra properties
         /*if (e.Properties != null && e.Properties.Count > 0)
         {
            Console.WriteLine();
            bool firstProp = true;
            foreach (var prop in e.Properties)
            {
               if (prop.Key == LogEvent.ErrorPropertyName) continue;

               if (!firstProp)
               {
                  Cg.Write(Pipe, Separator);
               }
               else
               {
                  firstProp = false;
               }

               Cg.Write(prop.Key, ConsoleColor.Gray);
               Cg.Write("='", Separator);
               Cg.Write(prop.Value.ToString(), ConsoleColor.Green);
               Cg.Write("'", Separator);
            }
         }*/

         Console.WriteLine();
      }

      private void LogSeverity(LogSeverity s)
      {
         switch(s)
         {
            case LogMagic.LogSeverity.Debug:
               Cg.Write("DBG", ConsoleColor.White);
               break;
            case LogMagic.LogSeverity.Error:
               Cg.Write("ERR", ConsoleColor.White, ConsoleColor.Red);
               break;
            case LogMagic.LogSeverity.Info:
               Cg.Write("INF", ConsoleColor.White, ConsoleColor.DarkGreen);
               break;
            case LogMagic.LogSeverity.Warning:
               Cg.Write("WRN", ConsoleColor.White, ConsoleColor.DarkRed);
               break;
         }
      }

      private string Abbreviate(string sourceName)
      {
         if(!Settings.AbbreviateClassNames)
         {
            if(_classNamePadding < sourceName.Length) _classNamePadding = sourceName.Length;
            return sourceName.PadRight(_classNamePadding);
         }


         //the result here is padded
         string result;
         if(SourceNameToShortName.TryGetValue(sourceName, out result)) return result;

         string abbreviated = new string(sourceName.Where(char.IsUpper).ToArray());
         if(_classNamePadding < abbreviated.Length) _classNamePadding = abbreviated.Length;
         abbreviated = abbreviated.PadRight(_classNamePadding);
         SourceNameToShortName[sourceName] = abbreviated;
         return abbreviated;
      }

      private class Cg : IDisposable
      {
         private readonly ConsoleColor _prevForeground;
         private readonly ConsoleColor? _prevBackground;

         private Cg(ConsoleColor foreground, ConsoleColor? background = null)
         {
            _prevForeground = Console.ForegroundColor;
            Console.ForegroundColor = foreground;

            if(background != null)
            {
               _prevBackground = Console.BackgroundColor; ;
               Console.BackgroundColor = background.Value;
            }
         }

         public static void Write(string value, ConsoleColor color, ConsoleColor? background = null)
         {
            using(new Cg(color, background))
            {
               Console.Write(value);
            }
         }

         public void Dispose()
         {
            Console.ForegroundColor = _prevForeground;
            if(_prevBackground != null)
            {
               Console.BackgroundColor = _prevBackground.Value;
            }
         }

      }

      /// <summary>
      /// Dispose
      /// </summary>
      public void Dispose()
      {
         //nothing to dispose in posh console
      }

      public void Write(IEnumerable<LogEvent> events)
      {
         lock (ColourLock)
         {
            foreach (LogEvent e in events)
            {
               Write(e);
            }
         }
      }

      public Task WriteAsync(IEnumerable<LogEvent> events)
      {
         Write(events);
         return Task.FromResult(true);
      }
   }
}
