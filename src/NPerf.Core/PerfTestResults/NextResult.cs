namespace NPerf.Core.PerfTestResults
{
    using System;

    /// <summary>
    /// Class describing benchmark results.
    /// </summary>
    [Serializable]
    public class NextResult : PerfTestResult
    {
        public NextResult()
        {
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public NextResult(double duration, long memoryUsage, double descriptor)
        {
            this.Duration = duration;
            this.MemoryUsage = memoryUsage;
            this.Descriptor = descriptor;
        }      

        /// <summary>
        /// Gets the test duration
        /// </summary>
        /// <value>
        /// Test duration
        /// </value>
        public double Duration { get; set; }

        public long MemoryUsage { get; set; }
        
        public override bool Equals(object obj)
        {
            var perfResult = obj as NextResult;
            if (perfResult == null)
            {
                return false;
            }

            return this.Descriptor.Equals(perfResult.Descriptor) && this.Duration.Equals(perfResult.Duration)
                   && this.MemoryUsage.Equals(perfResult.MemoryUsage) && base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() + (int)this.Descriptor;
        }
    }
}
