namespace NPref.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using NPerf.Framework;

    /// <summary>
    /// The multy test runner.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class SingleTestMultyRunner<T> : ISingleTestRunner
    {
        public SingleTestMultyRunner(IFixture<T> fixture, string testName, IPerfMonitorsManager monitors, params T[] testedObjects)
        {
        }


        public void RunTest(int index)
        {
            throw new NotImplementedException();
        }
    }
}
