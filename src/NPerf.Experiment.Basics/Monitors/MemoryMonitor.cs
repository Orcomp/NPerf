namespace NPerf.Experiment.Basics.Monitors
{
    using System.Diagnostics;

    internal class MemoryMonitor : BasePerfMonitor
    {
        private readonly PerformanceCounter perfCounter;

        private MemoryStatus startStatus;

        private MemoryStatus endStatus;

        public MemoryMonitor(string instanceName)
            : base(EPerfCounterType.Memory, instanceName)
        {
        }

        public override void Start()
        {
            this.startStatus = new MemoryStatus(Process.GetCurrentProcess());
            this.endStatus = null;
        }

        public override void Stop()
        {
            this.endStatus = new MemoryStatus(Process.GetCurrentProcess());
            this.perfCounter.RawValue = this.endStatus.TotalMemory - this.startStatus.TotalMemory; 
        }
    }
}
