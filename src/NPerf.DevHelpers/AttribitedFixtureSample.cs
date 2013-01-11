namespace NPerf.DevHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using NPerf.Framework;

    [PerfTester(typeof(ITestedObject), 10, "Tester description string", "Tester feature description")]
    public class AttribitedFixtureSample
    {
        [PerfSetUp]
        public void SetUp(int iteration, ITestedObject testedObject)
        {
        }

        [PerfTearDown]
        public void TearDown(ITestedObject testedObject)
        {
        }

        [PerfTest("Test1 description string")]
        public void Test(ITestedObject testedObject)
        {
        }

        [PerfTest("Test2 description string")]
        public void Test2(ITestedObject testedObject)
        {
        }

        [PerfTest("IgnoredTest1 description string")]
        [PerfIgnore("Ignored test message")]
        public void IgnoredTest1(ITestedObject testedObject)
        {
        }

        [PerfTest("IgnoredTest2 description string")]
        [PerfIgnore("Ignored test message")]
        public void IgnoredTest2(ITestedObject testedObject)
        {
        }

        [PerfRunDescriptor]
        public double GetRunDesctiptor(int iteration)
        {
            return iteration;
        }
    }
}
