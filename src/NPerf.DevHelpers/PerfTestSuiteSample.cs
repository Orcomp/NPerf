namespace NPerf.DevHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using NPerf.Framework;
    using NPerf.Framework.Interfaces;
    using NPerf.Core;
    using NPerf.Test.Helpers;

    public class PerfTestSuiteSample : AbstractPerfTestSuite<ITestedObject>
    {
        public PerfTestSuiteSample()
        {
            this.DefaultTestCount = 10;
            this.TestSuiteDescription = "PerfTestSuiteSample description";
            this.GetDescriptorMethod = i => i / 2d;
            this.SetUpMethod = (i, obj) => { };
            this.TearDownMethod = obj => { };
            this.Tests = new[] { new PerfTestSample(obj => { }, "TestMethod", "test description") };
        }
    }
}
