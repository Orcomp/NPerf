namespace NPerf.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public delegate void TestMethod<T>(T testedObject);

    public interface ISinglePerfTest<T>
    {
        string Name { get; }

        void Test(T testedObject);
    }
}
