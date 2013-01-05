namespace NPerf.Core
{
    using System;
    using System.Xml;
    using System.Xml.Serialization;
    using System.Collections.Generic;

    [XmlRoot("run")]
    public class PerfTestRun
    {
        public PerfTestRun()
        {
            this.Value = 0;
            this.Results = new List<PerfResult>();
            this.FailedResults = new List<PerfFailedResult>();
        }

        /// <summary>
        /// </summary>
        public PerfTestRun(double value)
        {
            this.Value = value;
            this.Results = new List<PerfResult>();
            this.FailedResults = new List<PerfFailedResult>();
        }

        [XmlAttribute("value")]
        public double Value { get; set; }

        [XmlArrayItem(ElementName = "result", Type = typeof(PerfResult))]
        [XmlArray(ElementName = "results")]
        public IList<PerfResult> Results { get; set; }

        [XmlArrayItem(ElementName = "failed-result", Type = typeof(PerfFailedResult))]
        [XmlArray(ElementName = "failed-results")]
        public IList<PerfFailedResult> FailedResults { get; set; }

    }
}
