using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogMagic.Writers
{
   /// <summary>
   /// Posh colorful console, you should use this if you want to look cool.
   /// </summary>
   class PoshConsoleLogWriter : ILogWriter
   {
      private static readonly ConcurrentDictionary<string, string> SourceNameToShortName = new ConcurrentDictionary<string, string>();
      private static object ColourLock = new object();

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
         Cg.Write(":", ConsoleColor.Gray);
         Cg.Write(e.EventTime.ToString("mm"), ConsoleColor.Green);
         Cg.Write(":", ConsoleColor.Gray);
         Cg.Write(e.EventTime.ToString("ss"), ConsoleColor.Green);
         Cg.Write(",", ConsoleColor.Gray);
         Cg.Write(e.EventTime.ToString("fff"), ConsoleColor.DarkGreen);

         //level
         Cg.Write("|", ConsoleColor.DarkGray);
         GetLogSeverity(e.Severity);

         //source
         Cg.Write("|", ConsoleColor.DarkGray);
         Cg.Write(Abbreviate(e.SourceName), ConsoleColor.Gray);

         if(e.Properties != null && e.Properties.Count > 0)
         {
            Console.WriteLine();
            bool firstProp = true;
            foreach (var prop in e.Properties)
            {
               if (!firstProp)
               {
                  Cg.Write("|", ConsoleColor.DarkGray);
               }
               else
               {
                  firstProp = false;
               }
               
               Cg.Write(prop.Key, ConsoleColor.Gray);
               Cg.Write("='", ConsoleColor.DarkGray);
               Cg.Write(prop.Value.ToString(), ConsoleColor.Green);
               Cg.Write("'", ConsoleColor.DarkGray);
            }
         }

         //message
         Cg.Write("|", ConsoleColor.DarkGray);
         Cg.Write(e.Message, GetMessageColor(e.Severity));

         //error
         object error = e.GetProperty(LogEvent.ErrorPropertyName);
         if(error != null)
         {
            Console.WriteLine();
            Cg.Write(error.ToString(), ConsoleColor.Red);
         }

         Console.WriteLine();
      }

      private void GetLogSeverity(LogSeverity s)
      {
         switch(s)
         {
            case LogSeverity.Debug:
               Cg.Write("D", ConsoleColor.Magenta);
               break;
            case LogSeverity.Error:
               Cg.Write("E", ConsoleColor.Red);
               break;
            case LogSeverity.Info:
               Cg.Write("I", ConsoleColor.Green);
               break;
            case LogSeverity.Warning:
               Cg.Write("W", ConsoleColor.DarkRed);
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

      private static ConsoleColor GetMessageColor(LogSeverity severity)
      {
         switch(severity)
         {
            default:
               return ConsoleColor.Green;
         }
      }

      private class Cg : IDisposable
      {
         private readonly ConsoleColor _prevColor;

         public Cg(ConsoleColor color)
         {
            _prevColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
         }

         public static void Write(string value, ConsoleColor color)
         {
            using(new Cg(color))
            {
               Console.Write(value);
            }
         }

         public void Dispose()
         {
            Console.ForegroundColor = _prevColor;
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
