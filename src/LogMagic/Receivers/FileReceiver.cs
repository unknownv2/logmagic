using LogMagic.Formatters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LogMagic.Receivers
{
   /// <summary>
   /// Simple file receiver.
   /// </summary>
   public class FileReceiver : AsyncReceiver
   {
      private readonly string _directoryName;
      private readonly string _fileNamePart;
      private readonly string _extensionPart;
      private readonly ILogChunkFormatter _formatter;
      private StreamWriter _writer;
      private int _fileYear;
      private int _fileMonth;
      private int _fileDay;

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
         if(fileName == null) throw new ArgumentNullException(nameof(fileName));

         SplitPath(fileName, out _directoryName, out _fileNamePart, out _extensionPart);
         if(!Directory.Exists(_directoryName)) Directory.CreateDirectory(_directoryName);

         PreCreateDirectory(fileName);

         _formatter = formatter ?? new StandardFormatter();
      }

      /// <summary>
      /// Contains the full file name of the current log file
      /// </summary>
      public string FileName { get; private set; }

      private static void SplitPath(string fullName, out string directory, out string file, out string ext)
      {
         int idx = fullName.LastIndexOf(Path.DirectorySeparatorChar);
         if(idx == -1)//file name can be just a name or format may be wrong
         {
            directory = null;
            file = Path.GetFileNameWithoutExtension(fullName);
            ext = Path.GetExtension(fullName);
            return;
         }

         directory = fullName.Substring(0, idx);
         file = fullName.Substring(idx + 1);
         ext = Path.GetExtension(file);
         file = Path.GetFileNameWithoutExtension(file);
      }

      private void PreCreateDirectory(string logFileName)
      {
         int idx = logFileName.LastIndexOf(Path.DirectorySeparatorChar);
         if (idx == -1) return;  //file name can be just a name or format may be wrong

         string dirPath = logFileName.Substring(0, idx);
         if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
      }

      private void OpenWriter(DateTime date)
      {
         _writer?.Dispose();

         FileName = Path.Combine(_directoryName, $"{_fileNamePart}-{date.ToString("yyyy-MM-dd")}{_extensionPart}");
         FileStream fs = File.Open(FileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
         _writer = new StreamWriter(fs, Encoding.UTF8, 1024, false);
      }

      /// <summary>
      /// Sends chunks
      /// </summary>
      protected override void SendChunks(IEnumerable<LogChunk> chunks)
      {
         foreach(LogChunk chunk in chunks)
         {
            CheckRolling(chunk.EventTime);

            _writer.Write(_formatter.Format(chunk));
         }
         _writer.Flush();
      }

      private void CheckRolling(DateTime date)
      {
         if(_writer == null || date.Year != _fileYear || date.Month != _fileMonth || date.Day != _fileDay)
         {
            OpenWriter(date);

            _fileYear = date.Year;
            _fileMonth = date.Month;
            _fileDay = date.Day;
         }
      }

      /// <summary>
      /// Closes the target file
      /// </summary>
      public override void Dispose()
      {
         _writer?.Dispose();

         base.Dispose();
      }
   }
}
