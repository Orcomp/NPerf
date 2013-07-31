namespace NPerf.Core.PerfTestResults
{
    using System;

    [Serializable]
    public class ExperimentCompleted : PerfTestResult
    {
        public ExperimentCompleted()
        {
            this.Descriptor = -1;
        }
    }
}
