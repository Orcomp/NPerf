namespace NPerf.Experiment.Basics.Monitors
{
    using NPerf.Framework.Interfaces;

    public class PerfMonitorsManager
    {
        public PerfMonitorsManager(string instance)
        {
            this.MemoryMonitor = new MemoryMonitor(instance);
            this.DurationMonitor = new DurationMonitor(instance);
            this.DescriptorMonitor = new DescriptorMonitor(instance);
        }

        public IPerfMonitor MemoryMonitor { get; private set; }

        public IPerfMonitor DurationMonitor { get; private set; }

        public IPerfMonitor DescriptorMonitor { get; private set; }
    }
}
