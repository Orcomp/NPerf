namespace NPerf.Core
{
    using System;

    /// <summary>
    /// Class describing benchmark results.
    /// </summary>
    [Serializable]
    public class PerfResult
    {
        public PerfResult()
        {
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public PerfResult(double duration, long memoryUsage)
        {
            this.Duration = duration;
            this.MemoryUsage = memoryUsage;
            this.Descriptor = RunDescriptor.Instance.Value;
        }

        public double Descriptor { get; set; }

        /// <summary>
        /// Gets the test duration
        /// </summary>
        /// <value>
        /// Test duration
        /// </value>
        public double Duration { get; set; }

        public long MemoryUsage { get; set; }
    }
}
