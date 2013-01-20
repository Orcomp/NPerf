namespace NPerf.Experiment.Basics.Monitors
{
    using NPerf.Framework.Interfaces;

    public abstract class BasePerfMonitor : IPerfMonitor
    {
        protected readonly IPerfCounter perfCounter;

        protected readonly EPerfCounterType counterType;

        public BasePerfMonitor(EPerfCounterType counterType, string instanceName, double scale = 1)
        {
            this.counterType = counterType;
            this.perfCounter = PerfCountersManager.Instance.GetPerfomanceCounter(counterType, instanceName, scale);
        }

        public abstract void Start();

        public abstract void Stop();
    }
}
