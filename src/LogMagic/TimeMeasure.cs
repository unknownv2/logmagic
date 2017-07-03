using System;
using System.Diagnostics;

namespace LogMagic
{
    /// <summary>
    /// Helps to measure time
    /// </summary>
    public class TimeMeasure : IDisposable
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private Exception _error;

        public TimeMeasure()
        {
            _stopwatch.Start();
        }

        public long ElapsedTicks => _stopwatch.Elapsed.Ticks;

        public TimeSpan Elapsed => _stopwatch.Elapsed;

        public void Dispose()
        {
            _stopwatch.Stop();
        }
    }
}