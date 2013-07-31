using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPerf.Framework;
using System.Collections;

namespace System.Perf.Dictionary
{
    [PerfTester(
        typeof(IDictionary), 15,
        Description = "IDictionary interface benchmark, addition and insertion", 
        FeatureDescription = "Number of elements")]
    public class EmptyDictionaryTester
    {
        private int count;
        private Random rnd = new Random();

        internal int CollectionCount(int testIndex)
        {
            int n = 0;

            if (testIndex < 0)
                n = 10;
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
        public void SetUp(int index, IDictionary dic)
        {
            this.count = CollectionCount(index);
        }

        [PerfTest]
        public void Add(IDictionary dic)
        {
            for (int i = 0; i < this.count; ++i)
                dic.Add(rnd.Next(), null);
        }

        [PerfTest]
        public void RandomItem(IDictionary dic)
        {
            for (int i = 0; i < this.count; ++i)
            {
                dic[rnd.Next()] = null;
            }
        }
    }
}
