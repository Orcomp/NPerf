namespace NPerf.Core.PerfTestResults
{
    using System;

    [Serializable]
    public class ExperimentCompleted : TestResult
    {
        public ExperimentCompleted()
        {
            this.Descriptor = -1;
        }
    }
}
