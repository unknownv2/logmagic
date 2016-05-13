using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace LogMagic.Receivers
{
   /// <summary>
   /// Uses standard Android logging
   /// </summary>
   public class AndroidLogReceiver : ILogReceiver
   {
      /// <summary>
      /// Create receiver with default formatter
      /// </summary>
      public AndroidLogReceiver()
      {
         
      }

      /// <summary>
      /// Sends chunk to the default Android logger
      /// </summary>
      /// <param name="chunk"></param>
      public void Send(LogChunk chunk)
      {

      }

      /// <summary>
      /// Nothing to dispose
      /// </summary>
      public void Dispose()
      {
      }

   }
}