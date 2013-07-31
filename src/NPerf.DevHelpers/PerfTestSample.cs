namespace NPerf.DevHelpers
{
    using System;
    using NPerf.Core;
    using NPerf.Test.Helpers;

    public class PerfTestSample : GenericPerfTest<ITestedObject>
    {
        public PerfTestSample(Action<ITestedObject> testMethod, string testName, string description)
        {
            this.TestDescription = description;
            this.TestMethodName = testName;
            this.TestMethod = testMethod;
        }
    }
}
