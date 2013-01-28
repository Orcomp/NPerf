namespace NPerf.Lab
{
    using System;
    using NPerf.Core;
    using NPerf.Core.PerfTestResults;

    public class ExperimentException:Exception
    {
        public ExperimentError Error { get; set; }
    }
}
