namespace NPerf.Core.PerfTestResults
{
    using System;

    public class PerfTestResultFactory
    {
        private static readonly PerfTestResultFactory instance = new PerfTestResultFactory();

        private Guid testId;

        public static PerfTestResultFactory Instance { get { return instance; } }

        public void Init(Guid testId)
        {
            this.testId = testId;
        }

        public PerfTestResult PerfResult(double duration, long memoryUsage, double descriptor)
        {
            return new NextResult(duration, memoryUsage, descriptor) { TestId = this.testId };
        }

        public PerfTestResult Comleted()
        {
            return new ExperimentCompleted { Descriptor = -1, TestId = this.testId };
        }

        public PerfTestResult FaultResult(double descriptor, Exception ex)
        {
            return new ExperimentError(ex) { Descriptor = descriptor, TestId = this.testId };
        }

        public PerfTestResult FatalError(Exception ex)
        {
            return new ExperimentError(ex) { Descriptor = -1, TestId = this.testId };
        }
    }
}
