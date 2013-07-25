namespace NPerf.Core
{
    using System;

    public abstract class GenericPerfTestSuite<T> : PerfTestSuite
    {
        public Action<int, T> SetUpMethod { get; protected set; }

        public Action<T> TearDownMethod { get; protected set; }        

        public override void SetUp(int iteration, object testedObject)
        {
            CheckTestedObject(testedObject);
            if (this.SetUpMethod != null)
            {
                this.SetUpMethod(iteration, (T)testedObject);
            }
        }

        public override void TearDown(object testedObject)
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
    }
}
