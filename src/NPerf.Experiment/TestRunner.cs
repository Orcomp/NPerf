namespace NPerf.Experiment
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using NPerf.Framework;

    internal class TestRunner
    {
        private IPerfFixture tool;

        private Action<object> testMethod;

        private object testedObject;

        public TestRunner(IPerfFixture tool, int testIndex, object testedObject)
        {
            this.tool = tool;
            this.testMethod = tool.Tests[testIndex].Test;
            this.testedObject = testedObject;
        }

        public void RunTests()
        {
                for (var i = 0; i < this.tool.DefaultTestCount; i++)
                {
                    this.tool.SetUp(i, this.testedObject);
                    using (new Monitoring(this.tool, i))
                    {
                        this.testMethod(this.testedObject);
                    }

                    this.tool.TearDown(this.testedObject);
                }
        }

        public void RunTests(int start, int end, int step)
        {
            for (var i = start; i < end; i += step)
            {
                this.tool.SetUp(i, this.testedObject);
                using (new Monitoring(this.tool, i))
                {
                    this.testMethod(this.testedObject);
                }

                this.tool.TearDown(this.testedObject);
            }
        }
    }
}
