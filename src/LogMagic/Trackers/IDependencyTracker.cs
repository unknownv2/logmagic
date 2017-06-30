using System;

namespace LogMagic.Trackers
{
   /// <summary>
   /// Tracks application dependencies
   /// </summary>
   public interface IDependencyTracker : IDisposable
   {
      /// <summary>
      /// Indicates that dependency has failed
      /// </summary>
      /// <param name="e">Dependency error</param>
      void Add(Exception e);
   }
}
