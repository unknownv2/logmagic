using System;

namespace LogMagic.Trackers
{
   /// <summary>
   /// Application request tracker
   /// </summary>
   public interface IRequestTracker : IDisposable
   {
      /// <summary>
      /// Indicates that request has failed
      /// </summary>
      /// <param name="e">Request error</param>
      void Add(Exception e);
   }
}