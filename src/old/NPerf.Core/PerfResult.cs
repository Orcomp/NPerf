namespace NPerf.Core
{
    using System;
    using System.Xml.Serialization;

    /// <summary>
    /// Class describing benchmark results.
    /// </summary>
    [Serializable]
    [XmlRoot("result")]
    public class PerfResult
    {
        public PerfResult()
        {
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public PerfResult(Type testedType, double duration, long memoryUsage)
        {
            if (testedType == null)
            {
                throw new ArgumentNullException("testedType");
            }

            this.TestedType = testedType.Name;
            this.Duration = duration;
            this.MemoryUsage = memoryUsage;
        }

        [XmlAttribute("tested-type")]
        public string TestedType { get; set; }

        /// <summary>
        /// Gets the test duration
        /// </summary>
        /// <value>
        /// Test duration
        /// </value>
        [XmlAttribute("duration")]
        public double Duration { get; set; }

        [XmlAttribute("memory-usage")]
        public long MemoryUsage { get; set; }

        [XmlIgnore]
        public double MemoryUsageMb
        {
            get
            {
                return (double)this.MemoryUsage / (1024 * 1024);
            }
        }
    }
}
