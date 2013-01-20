namespace NPerf.Experiment.Basics.Monitors
{
    using NPerf.Experiment.Basics;

    public class DescriptorMonitor : BasePerfMonitor
    {
        public DescriptorMonitor(string instanceName)
            : base(EPerfCounterType.Duration, instanceName, 1000)
        {
        }

        public override void Start()
        {
            
        }

        public override void Stop()
        {
            this.perfCounter.Value = Descriptor.Instance.Value;
        }
    }
}
