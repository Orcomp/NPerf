namespace NPref.Fixture
{
    using System;
    using NPerf.Framework;

    public class PerfTest : IPerfTest
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsIgnore { get; set; }

        public string IgnoreMessage { get; set; }

        public Action<object> TestMethod { get; set; }

        public void Test(object testedObject)
        {
            this.TestMethod(testedObject);
        }
    }
}
