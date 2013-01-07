namespace NPref.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using NPerf.Framework;

    /// <summary>
    /// single test for single object runner
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class SingleTestRunner<T> : ISingleTestRunner
    {
        private readonly IFixture<T> fixture;

        private readonly T testedObject;

        private readonly TestMethod<T> testMethod;

        private readonly IPerfMonitorsManager monitors;

        public SingleTestRunner(IFixture<T> fixture, string testName, IPerfMonitorsManager monitors, T testedObject)
        {
            this.fixture = fixture;
            this.testedObject = testedObject;
            this.testMethod = fixture.Tests.First(test => test.Name == testName).Test;
            this.monitors = monitors;
        }

        public void RunTest(int index)
        {
            this.fixture.SetUp(index, this.testedObject);
            using (this.monitors.Observe())
            {
                this.testMethod(this.testedObject);                
            }

            this.fixture.TearDown(this.testedObject);
        }
    }
}
