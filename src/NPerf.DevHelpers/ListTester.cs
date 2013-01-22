using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPerf.Framework;

namespace NPerf.DevHelpers
{
    [PerfTester(typeof(ITestedObject), 50)]
    public class ListTester
    {
        [PerfSetUp]
        public void SetUp(int iteration, IList<int> testedObject)
        {
            
        }

        [PerfTearDown]
        public void TearDown(IList<int> testedObject)
        {
            
        }

        [PerfRunDescriptor]
        public double GetRunDesctiptor(int iteration)
        {
            return iteration;
        }

        [PerfTest("")]
        public void Add(IList<int> testedObject)
        {
            
        }
    }
}
