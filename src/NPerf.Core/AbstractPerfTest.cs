namespace NPerf.Core
{
    using System;
    using NPerf.Framework.Interfaces;

    public abstract class AbstractPerfTest<T> : IPerfTest
    {
        public string TestMethodName { get; protected set; }

        public string TestDescription { get; protected set; }

        public Action<T> TestMethod { get; protected set; }

        public Guid TestId { get; protected set; }

        public void Test(object testedObject)
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
