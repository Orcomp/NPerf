namespace NPerf.DevHelpers
{
    using System;
    using NPerf.Core;

    public class PerfTestSample : AbstractPerfTest<ITestedObject>
    {
        public PerfTestSample(Action<ITestedObject> testMethod, string testMethodName, string testName, string description)
        {
            this.Description = description;
            this.Name = testName;
            this.TestMethodName = testMethodName;
            this.TestMethod = testMethod;
        }
    }
}
