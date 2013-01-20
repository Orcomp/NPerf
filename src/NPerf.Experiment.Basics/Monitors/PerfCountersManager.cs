namespace NPerf.Experiment.Basics.Monitors
{
    using System.Diagnostics;

    using NPerf.Framework.Interfaces;

    public class PerfCountersManager
    {
        protected const string CategoryName = "NPerf";

        protected readonly PerformanceCounterCategory Category;

        public readonly static PerfCountersManager Instance = new PerfCountersManager();

        private PerfCountersManager()
        {
            if (!PerformanceCounterCategory.Exists(CategoryName))
            {
                var counters = new CounterCreationDataCollection 
                { 
                    new CounterCreationData
                        {
                            CounterName = EPerfCounterType.Memory.ToString(), 
                            CounterType = PerformanceCounterType.NumberOfItems64
                        },
                    new CounterCreationData
                        {
                            CounterName = EPerfCounterType.Duration.ToString(),
                            CounterType = PerformanceCounterType.NumberOfItems64
                        },
                    new CounterCreationData
                        {
                            CounterName = EPerfCounterType.Descriptor.ToString(),
                            CounterType = PerformanceCounterType.NumberOfItems64
                        }
                };

                this.Category = PerformanceCounterCategory.Create(CategoryName, "NPerf counter", PerformanceCounterCategoryType.MultiInstance, counters);
            }
            else
            {
                this.Category = new PerformanceCounterCategory(CategoryName);
            }            
        }

        public IPerfCounter GetPerfomanceCounter(EPerfCounterType counterType, string instanceName, double scale)
        {
            return new PerfCounter(new PerformanceCounter(CategoryName, counterType.ToString(), instanceName), scale);
        }
    }
}
