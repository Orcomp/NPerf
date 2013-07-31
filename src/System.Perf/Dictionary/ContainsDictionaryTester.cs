using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPerf.Framework;
using System.Collections;

namespace System.Perf.Dictionary
{
    [PerfTester(typeof(IDictionary), 14
     , Description = "IDictionary interface benchmark, contains and enumeration"
     , FeatureDescription = "Number of elements")]
    public class ContainsDictionaryTester
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
            for (int i = 0; i < this.count; ++i)
                dic[rnd.Next()] = null;
        }

        [PerfTest]
        public void Contains(IDictionary dic)
        {
            foreach (DictionaryEntry de in dic)
            {
                bool b = dic.Contains(de.Key);
            }
        }

        [PerfTest]
        public void Enumerate(IDictionary dic)
        {
            foreach (DictionaryEntry de in dic)
            {
                object o = de.Key;
            }
        }
    }
}
