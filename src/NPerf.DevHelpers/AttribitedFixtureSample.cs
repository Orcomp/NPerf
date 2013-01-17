namespace NPerf.DevHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using NPerf.Framework;

    [PerfTester(typeof(ITestedObject), 6000, "Tester description string", "Tester feature description")]
    public class AttribitedFixtureSample
    {
        [PerfSetUp]
        public void SetUp(int iteration, ITestedObject testedObject)
        {
            Console.Out.WriteLine("SetUp executed. {0}", iteration);
        }

        [PerfTearDown]
        public void TearDown(ITestedObject testedObject)
        {
            Console.Out.WriteLine("TearDown executed.");
        }

        [PerfTest("Test1 description string")]
        public void Test(ITestedObject testedObject)
        {
            Console.Out.WriteLine("Test executed.");
        }

        [PerfTest("Test2 description string")]
        public void Test2(ITestedObject testedObject)
        {
            Console.Out.WriteLine("Test2 executed.");
        }

        [PerfTest("IgnoredTest1 description string")]
        [PerfIgnore("Ignored test message")]
        public void IgnoredTest1(ITestedObject testedObject)
        {
            Console.Out.WriteLine("IgnoredTest1 executed.");
        }

        [PerfTest("IgnoredTest2 description string")]
        [PerfIgnore("Ignored test message")]
        public void IgnoredTest2(ITestedObject testedObject)
        {
            Console.Out.WriteLine("IgnoredTest2 executed.");
        }

        [PerfRunDescriptor]
        public double GetRunDesctiptor(int iteration)
        {
            return iteration;
        }
    }
}
