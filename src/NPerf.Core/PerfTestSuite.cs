using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPerf.Core
{
    public abstract class PerfTestSuite
    {
        public PerfTest[] Tests { get; protected set; }   

        public int DefaultTestCount { get; protected set; }

        public string TestSuiteDescription { get; protected set; }

        public string FeatureDescription { get; protected set; }

        public Type TestedAbstraction { get; protected set; }

        public Func<int, double> DescriptorGetter { get; protected set; }

        public virtual double GetRunDescriptor(int iteration)
        {
            return this.DescriptorGetter != null
                                            ? this.DescriptorGetter(iteration)
                                            : iteration;
        }

        public abstract void SetUp(int iteration, object testedObject);

        public abstract void TearDown(object testedObject);
    }
}
