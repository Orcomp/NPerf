namespace NPerf.Experiment.Basics.Monitors
{
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using System.Threading;

    internal class DurationMonitor : BasePerfMonitor
    {
        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(out long lpFrequency);

        private long startTime;

        private long stopTime;

        public DurationMonitor(string instanceName)
            : base(EPerfCounterType.Duration, instanceName)
        {
            if (QueryPerformanceFrequency(out this.freq) == false)
            {
                // high-performance counter not supported 
                throw new Win32Exception();
            }
        }

        private readonly long freq;

        public override void Start()
        {
            // lets do the waiting threads there work
            Thread.Sleep(0);

            QueryPerformanceCounter(out this.startTime);
        }

        public override void Stop()
        {
            QueryPerformanceCounter(out this.stopTime);
            this.perfCounter.Value = (long)(10000L * ((this.stopTime - this.startTime) / (double)this.freq));
        }
    }
}
