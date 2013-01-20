namespace NPerf.Experiment
{
    using System;
    using NPerf.Framework.Interfaces;

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
                    using (new PerfObserver(this.suite))
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

                // clean memory
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

                using (new PerfObserver(this.suite))
                {
                    this.testMethod(this.testedObject);
                }

                this.suite.TearDown(this.testedObject);
            }
        }
    }
}
