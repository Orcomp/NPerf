namespace NPerf.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IFixture<T>
    {
        ISinglePerfTest<T>[] Tests { get; }

        void SetUp(int index, T testedObject);        

        double RunDescriptor(T testedObject);

        void TearDown(T testedObject);

        int TestCount { get; }

        string Description { get; }

        string FeatureDescription { get; }
    }
}
