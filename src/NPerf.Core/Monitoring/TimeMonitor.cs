namespace NPerf.Core.Monitoring
{
    using System;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using System.Threading;

    /// <summary>
    /// A high performance timer
    /// </summary>
    /// <remarks>
    /// High Precision Timer based on Win32 methods.
    /// </remarks>
    public class TimeMonitor
    {
        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(out long lpFrequency);
               
        private long startTime, stopTime;

        private long now;

        private readonly long freq;

        // Constructor
        public TimeMonitor()
        {
            var c = new System.Diagnostics.PerformanceCounter();
            
            startTime = 0;
            stopTime = 0;

            if (QueryPerformanceFrequency(out this.freq) == false)
            {
                // high-performance counter not supported 
                throw new Win32Exception();
            }
        }

        public long Frequency
        {
            get
            {
                return this.freq;
            }
        }

        // Start the timer
        public void Start()
        {
            // lets do the waiting threads there work
            Thread.Sleep(0);

            QueryPerformanceCounter(out this.startTime);
        }

        // Stop the timer
        public void Stop()
        {
            QueryPerformanceCounter(out this.stopTime);
        }

        public double Now
        {
            get
            {
                QueryPerformanceCounter(out this.now);
                return (this.now - this.startTime) / (double)this.freq;
            }
        }

        // Returns the duration of the timer (in seconds)
        public double Duration
        {
            get
            {
                return (this.stopTime - this.startTime) / (double)this.freq;
            }
        }
    }
}
