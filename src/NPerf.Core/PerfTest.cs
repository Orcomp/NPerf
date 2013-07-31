using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPerf.Core
{
    public abstract class PerfTest
    {
        public string TestMethodName { get; protected set; }

        public string TestDescription { get; protected set; }

        public Guid TestId { get; protected set; }

        public abstract void Test(object testedObject);

        public Type TestedType { get; set; }
    }
}
