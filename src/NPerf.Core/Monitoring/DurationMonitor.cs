namespace NPerf.Core.Monitoring
{
    using System;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using System.Threading;

    public class DurationMonitor : IPerfMonitor<double>
    {
        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(out long lpFrequency);

        private long startTime;

        private long stopTime;

        public DurationMonitor()
        {
            if (QueryPerformanceFrequency(out this.freq) == false)
            {
                // high-performance counter not supported 
                throw new Win32Exception();
            }
        }

        private readonly long freq;

        public IDisposable Observe()
        {
            return new DisposableObserver(
                () =>
                    {
                        // lets do the waiting threads there work
                        Thread.Sleep(0);

                        QueryPerformanceCounter(out this.startTime);
                    },
                () =>
                    {
                        QueryPerformanceCounter(out this.stopTime);
                        this.Value = (this.stopTime - this.startTime) / (double)this.freq;
                    });
        }

        public double Value { get; private set; }
    }
}
