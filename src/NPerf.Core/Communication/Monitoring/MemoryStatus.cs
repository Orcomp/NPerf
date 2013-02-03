namespace NPerf.Core.Monitoring
{
    using System;
    using System.Diagnostics;

    class MemoryStatus
    {
        public MemoryStatus(Process p)
        {
            this.WorkingSet = p.WorkingSet64;
            this.PeakWorkingSet = p.PeakWorkingSet64;
            this.TotalMemory = GC.GetTotalMemory(false);
        }

        public long WorkingSet { get; private set; }

        public long PeakWorkingSet { get; private set; }

        public long TotalMemory { get; private set; }
    }
}
