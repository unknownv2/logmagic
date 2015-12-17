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
      private readonly ILogChunkFormatter _formatter;
      private StreamWriter _writer;

      /// <summary>
      /// Creates an instance of file receiver
      /// </summary>
      /// <param name="fileName">Target filename. If file does not exist it will be created.</param>
      public FileReceiver(string fileName) : this(fileName, null)
      {

      }

      /// <summary>
      /// Creates an instance of file receiver
      /// </summary>
      /// <param name="fileName">Target filename. If file does not exist it will be created.</param>
      /// <param name="formatter">Optional chunk formatter</param>
      public FileReceiver(string fileName, ILogChunkFormatter formatter)
      {
         if (fileName == null) throw new ArgumentNullException(nameof(fileName));

         //todo: create directory if it doesn't exist

         _formatter = formatter ?? new StandardFormatter();
         _writer = File.Exists(fileName) ? File.AppendText(fileName) : File.CreateText(fileName);
      }

      /// <summary>
      /// Sends chunks
      /// </summary>
      protected override void SendChunks(IEnumerable<LogChunk> chunks)
      {
         foreach(LogChunk chunk in chunks)
         {
            _writer.Write(_formatter.Format(chunk));
         }
         _writer.Flush();
      }

      /// <summary>
      /// Closes the target file
      /// </summary>
      public override void Dispose()
      {
         _writer.Dispose();

         base.Dispose();
      }
   }
}
