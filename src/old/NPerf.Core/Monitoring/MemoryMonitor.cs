namespace NPerf.Core.Monitoring
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Summary description for MemoryTracker.
    /// </summary>
    public class MemoryMonitor
    {
        public MemoryStatus StartStatus { get; private set; }

        public MemoryStatus EndStatus { get; private set; }

        public long Usage
        {
            get
            {
                return this.EndStatus.TotalMemory - this.StartStatus.TotalMemory;
            }
        }

        public void Start()
        {
            this.StartStatus = new MemoryStatus(Process.GetCurrentProcess());
            this.EndStatus = null;
        }

        public void Stop()
        {
            this.EndStatus = new MemoryStatus(Process.GetCurrentProcess());
        }
    }
}
