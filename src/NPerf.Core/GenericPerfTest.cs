namespace NPerf.Core
{
    using System;

    public abstract class GenericPerfTest<T> : PerfTest
    {
        public Action<T> TestMethod { get; protected set; }

        public override void Test(object testedObject)
        {
            if (testedObject == null)
            {
                throw new ArgumentNullException("testedObject");
            }

            if (this.TestMethod == null)
            {
                throw new InvalidOperationException("TestMethod not initialized.");
            }

            this.TestMethod((T)testedObject);
        }
    }
}
