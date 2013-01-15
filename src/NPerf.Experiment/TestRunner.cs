namespace NPerf.Experiment
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using NPerf.Framework;

    internal class TestRunner
    {
        private IPerfTestSuite suite;

        private Action<object> testMethod;

        private object testedObject;

        public TestRunner(IPerfTestSuite suite, int testIndex, object testedObject)
        {
            this.suite = suite;
            this.testMethod = suite.Tests[testIndex].Test;
            this.testedObject = testedObject;
        }

        public void RunTests()
        {
                for (var i = 0; i < this.suite.DefaultTestCount; i++)
                {
                    this.suite.SetUp(i, this.testedObject);
                    using (new Monitoring(this.suite, i))
                    {
                        this.testMethod(this.testedObject);
                    }

                    this.suite.TearDown(this.testedObject);
                }
        }

        public void RunTests(int start, int end, int step)
        {
            for (var i = start; i < end; i += step)
            {
                this.suite.SetUp(i, this.testedObject);
                using (new Monitoring(this.suite, i))
                {
                    this.testMethod(this.testedObject);
                }

                this.suite.TearDown(this.testedObject);
            }
        }
    }
}
