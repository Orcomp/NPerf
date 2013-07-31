using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPerf.Framework;

namespace System.Perf.DelegateInterface
{
    [PerfTester(
        typeof(IMethodCaller),
        50,
        Description = "Comparing interface and delegate calls",
        FeatureDescription = "Method call number")]
    public class DelegateInterfaceTester
    {
        internal int CallCount(int index)
        {
            if (index < 0)
                return 10;
            else
                return (int)Math.Floor(Math.Pow(index, 2) * 1000);
        }

        [PerfRunDescriptor]
        public double RunDescriptor(int index)
        {
            return (double)CallCount(index);
        }

        [PerfSetUp]
        public void SetUp(int index, IMethodCaller mc)
        {
            mc.SetMethod(new MethodCall(), CallCount(index));
        }

        [PerfTest()]
        public void Call(IMethodCaller mc)
        {
            mc.CallMethod();
        }
    }
}
