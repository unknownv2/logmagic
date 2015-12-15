using LogMagic.Formatters;
using System;
using System.Collections.Generic;
using System.IO;

namespace LogMagic.Receivers
{
   /// <summary>
   /// Simple file logger
   /// </summary>
   public class FileReceiver : AsyncReceiver
   {
      private readonly IFormatter _formatter;
      private StreamWriter _writer;

      public FileReceiver(string fileName) : this(fileName, null)
      {

      }

      public FileReceiver(string fileName, IFormatter formatter)
      {
         if (fileName == null) throw new ArgumentNullException(nameof(fileName));

         _formatter = formatter ?? new StandardFormatter();
         _writer = File.CreateText(fileName);
      }

      protected override void SendChunks(IEnumerable<LogChunk> chunks)
      {
         foreach(LogChunk chunk in chunks)
         {
            _writer.Write(_formatter.Format(chunk));
         }
      }

      public override void Dispose()
      {
         _writer.Dispose();

         base.Dispose();
      }
   }
}
