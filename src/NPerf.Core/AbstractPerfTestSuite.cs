namespace NPerf.Core
{
    using System;
    using NPerf.Framework.Interfaces;

    public abstract class AbstractPerfTestSuite<T> : IPerfTestSuite
    {
        public IPerfTest[] Tests { get; protected set; }

        public int DefaultTestCount { get; protected set; }

        public string TestSuiteDescription { get; protected set; }

        public string FeatureDescription { get; protected set; }

        public Type TestedType { get; protected set; }

        public Type TestedAbstraction { get; protected set; }

        public Action<int, T> SetUpMethod { get; protected set; }

        public Action<T> TearDownMethod { get; protected set; }

        public Func<int, double> GetDescriptorMethod { get; protected set; }

        public void SetUp(int iteration, object testedObject)
        {
            CheckTestedObject(testedObject);
            if (this.SetUpMethod != null)
            {
                this.SetUpMethod(iteration, (T)testedObject);
            }
        }

        public void TearDown(object testedObject)
        {
            CheckTestedObject(testedObject);
            if (this.TearDownMethod != null)
            {
                this.TearDownMethod((T)testedObject);
            }
        }

        private static void CheckTestedObject(object testedObject)
        {
            if (testedObject == null)
            {
                throw new ArgumentNullException("testedObject");
            }

            if (!(testedObject is T))
            {
                throw new ArgumentException(string.Format("Tested object must be instance of {0}", typeof(T)));
            }
        }


        public double GetRunDescriptor(int iteration)
        {
            return this.GetDescriptorMethod != null
                                            ? this.GetDescriptorMethod(iteration)
                                            : iteration;
        }
    }
}
