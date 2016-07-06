﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace LogMagic.Writers
{
   /// <summary>
   /// Posh colorful console, you should use this if you want to look cool.
   /// </summary>
   class PoshConsoleLogWriter : ILogWriter
   {
      private static readonly ConcurrentDictionary<string, string> SourceNameToShortName = new ConcurrentDictionary<string, string>();

      /// <summary>
      /// Receiver's settings
      /// </summary>
      public PoshConsoleLogReceiverSettings Settings { get; private set; }

      private int _classNamePadding = 10;

      /// <summary>
      /// Constructs and instance of this class
      /// </summary>
      public PoshConsoleLogWriter()
      {
         Settings = new PoshConsoleLogReceiverSettings
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
         string threadId = Thread.CurrentThread.Name;
         if (string.IsNullOrEmpty(threadId)) threadId = Thread.CurrentThread.ManagedThreadId.ToString();

         //timestamp
         Cg.Write(e.EventTime.ToString("HH"), ConsoleColor.Green);
         Cg.Write(":", ConsoleColor.Gray);
         Cg.Write(e.EventTime.ToString("mm"), ConsoleColor.Green);
         Cg.Write(":", ConsoleColor.Gray);
         Cg.Write(e.EventTime.ToString("ss"), ConsoleColor.Green);
         Cg.Write(",", ConsoleColor.Gray);
         Cg.Write(e.EventTime.ToString("fff"), ConsoleColor.DarkGreen);

         //node ID
         if (!string.IsNullOrEmpty(L.NodeId))
         {
            Cg.Write("|", ConsoleColor.DarkGray);
            Cg.Write(L.NodeId.PadRight(10, ' '), ConsoleColor.Red);
         }

         //level
         Cg.Write("|", ConsoleColor.DarkGray);
         GetLogSeverity(e.Severity);

         //source
         Cg.Write("|", ConsoleColor.DarkGray);
         Cg.Write(Abbreviate(e.SourceName), ConsoleColor.Gray);

         //thread ID
         Cg.Write("|", ConsoleColor.DarkGray);
         Cg.Write(threadId.PadLeft(4, ' '), ConsoleColor.Blue);

         //message
         Cg.Write("|", ConsoleColor.DarkGray);
         Cg.Write(e.Message, GetMessageColor(e.Severity));

         //error
         if(e.Error != null)
         {
            Console.WriteLine();
            Cg.Write(e.Error.ToString(), ConsoleColor.Red);
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
         foreach(LogEvent e in events)
         {
            Write(e);
         }
      }
   }
}