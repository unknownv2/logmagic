using LogMagic.Formatters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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

         PreCreateDirectory(fileName);

         _formatter = formatter ?? new StandardFormatter();

         _writer = OpenWriter(fileName);
      }

      private void PreCreateDirectory(string logFileName)
      {
         int idx = logFileName.LastIndexOf(Path.DirectorySeparatorChar);
         if (idx == -1) return;  //file name can be just a name or format may be wrong

         string dirPath = logFileName.Substring(0, idx);
         if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
      }

      private StreamWriter OpenWriter(string fileName)
      {
         FileStream fs = File.Open(fileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
         return new StreamWriter(fs, Encoding.Unicode, 1024, false);
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
