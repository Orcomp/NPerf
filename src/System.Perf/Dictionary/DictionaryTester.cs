using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPerf.Framework;
using System.Collections;

namespace System.Perf.Dictionary
{
    [PerfTester(typeof(IDictionary), 10)]
    public class DictionaryTester
    {
        private int count = 0;
        private Random rnd = new Random();
        [PerfRunDescriptor]
        public double Count(int index)
        {
            return index * 1000;
        }

        [PerfSetUp]
        public void SetUp(int index, IDictionary dic)
        {
            this.count = (int)Math.Floor(Count(index));
        }

        [PerfTest]
        public void ItemAssign(IDictionary dic)
        {
            for (int i = 0; i < this.count; ++i)
                dic[rnd.Next()] = null;
        }
    }
}
