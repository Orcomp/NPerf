using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPerf.Framework;

namespace System.Perf.StringBuilding
{
    [PerfTester(
        typeof(IStringRunner), 13,
        Description = "Testing build construction",
        FeatureDescription = "Number of concatenations")]
    public class StringBuildingTester
    {
        private int count;
        private Random rnd = new Random();

        internal int CollectionCount(int testIndex)
        {
            int n = 0;

            if (testIndex < 0)
                n = 3;
            else
                n = (int)Math.Pow(2, testIndex);

            return n;
        }

        [PerfRunDescriptor]
        public double RunDescription(int testIndex)
        {
            return (double)CollectionCount(testIndex);
        }

        [PerfSetUp]
        public void SetUp(int index, IStringRunner dic)
        {
            this.count = CollectionCount(index);
        }

        [PerfTest]
        public void Concat(IStringRunner sr)
        {
            string s = "myword";
            for (int i = 0; i < count; ++i)
                sr.Concat(s);
        }


        [PerfTest]
        public void ConcatFormat(IStringRunner sr)
        {
            string format = "{0} {1}";
            string s1 = "myword";
            string s2 = "myotherword";
            for (int i = 0; i < count; ++i)
                sr.ConcatFormat(format, s1, s2);
        }

    }
}
