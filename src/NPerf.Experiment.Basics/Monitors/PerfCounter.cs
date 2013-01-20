namespace NPerf.Experiment.Basics.Monitors
{
    using System.Diagnostics;
    using NPerf.Framework.Interfaces;

    public class PerfCounter : IPerfCounter
    {
        private readonly PerformanceCounter counter;

        private readonly double valueScale;

        public PerfCounter(PerformanceCounter counter, double valueScale)
        {
            this.counter = counter;
            this.valueScale = valueScale;
        }

        public double Value
        {
            get
            {
                return this.counter.RawValue / this.valueScale;
            }

            set
            {
                this.counter.RawValue = (long)(value * this.valueScale);
            }
        }
    }
}
