namespace NPerf.Experiment
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using NPerf.Framework;
    using System.Windows.Forms;

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
                    //MessageBox.Show(string.Format("Setup {0}", i));
                    this.suite.SetUp(i, this.testedObject);
                    using (new Monitoring(this.suite, i))
                    {
                        //MessageBox.Show(string.Format("Before test {0}", i));
                        this.testMethod(this.testedObject);
                        //MessageBox.Show(string.Format("After test {0}", i));
                    }
                    
                    this.suite.TearDown(this.testedObject);
                    //MessageBox.Show(string.Format("Teardown {0}", i));
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
