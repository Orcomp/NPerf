namespace NPerf.Core.Monitoring
{
    using System;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Diagnostics;

    public class DurationMonitor : IPerfMonitor<double>
    {
        Stopwatch clock = new Stopwatch();

        public DurationMonitor()
        {
            this.freq = Stopwatch.Frequency;
            if (!Stopwatch.IsHighResolution)
            {
                throw new Win32Exception();
            }
        }

        private readonly long freq;

        public IDisposable Observe()
        {
            return new DisposableScope(
                () =>
                    {                        
                        Thread.Sleep(0);
                        clock.Restart();
                    },
                () =>
                    {
                        clock.Stop();
                        this.Value = clock.ElapsedTicks / (double)this.freq;
                    });
        }

        public double Value { get; private set; }
    }
}
