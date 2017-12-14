using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace LogMagic
{
   /// <summary>
   /// Helps to measure time
   /// </summary>
   public class TimeMeasure : IDisposable
   {
      private readonly Stopwatch _stopwatch = new Stopwatch();
      private Exception _error;

      /// <summary>
      /// Creates a new intance of time measure and starts measuring execution time
      /// </summary>
      public TimeMeasure()
      {
         _stopwatch.Start();
      }

      /// <summary>
      /// Gets number of ticks elapsed from the time this measure was created
      /// </summary>
      public long ElapsedTicks => _stopwatch.Elapsed.Ticks;

      /// <summary>
      /// Gets time elapsed from the time this measure was created
      /// </summary>
      public TimeSpan Elapsed => _stopwatch.Elapsed;

      /// <summary>
      /// Stops time measure
      /// </summary>
      public virtual void Dispose()
      {
         _stopwatch.Stop();
      }
   }
}