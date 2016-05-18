using System;
using System.Threading;
using Android.Util;

namespace LogMagic.Receivers
{
   class AndroidLogReceiver : ILogReceiver
   {
      private readonly string _appName;

      /// <summary>
      /// Create a new instance of log receiver
      /// </summary>
      /// <param name="appName">Application name used as TAG in android logging. It's recommened to use app name so you can apply log
      /// filtering based on it, instad of class name as Android is not that clever.</param>
      public AndroidLogReceiver(string appName)
      {
         if(appName == null) throw new ArgumentNullException(nameof(appName));
         _appName = appName;
      }

      public void Send(LogChunk chunk)
      {
         //this is the correct way to get current thread ID in Android
         int androidThreadId = Thread.CurrentThread.ManagedThreadId;

         string message = $"{androidThreadId}|{chunk.SourceName}|{chunk.Message}";
         if(chunk.Error != null) message += $":{chunk.Error}";

         Log.WriteLine(ToLogPriority(chunk.Severity), _appName, message);
      }

      private static LogPriority ToLogPriority(LogSeverity severity)
      {
         switch(severity)
         {
            case LogSeverity.Debug:
               return LogPriority.Debug;
            case LogSeverity.Error:
               return LogPriority.Error;
            case LogSeverity.Info:
               return LogPriority.Error;
            case LogSeverity.Warning:
               return LogPriority.Warn;
            default:
               return LogPriority.Verbose;
         }
      }

      public void Dispose()
      {
      }

   }
}