namespace NPerf.Core.Monitoring
{
    using System.Diagnostics;
    using System;

    public class MemoryMonitor : IPerfMonitor<long>
    {
        private MemoryStatus startStatus;

        private MemoryStatus endStatus;


        public IDisposable Observe()
        {
            return new DisposableScope(
                () =>
                {
                    this.startStatus = new MemoryStatus(Process.GetCurrentProcess());
                    this.endStatus = null;  
                },
                () =>
                {
                    this.endStatus = new MemoryStatus(Process.GetCurrentProcess());
                    this.Value = this.endStatus.TotalMemory - this.startStatus.TotalMemory; 
                });
        }

        public long Value { get; private set; }
    }
}
