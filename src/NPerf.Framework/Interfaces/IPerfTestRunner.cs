namespace NPerf.Framework.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IPerfTestRunner
    {
        IPerfTestSuite TestSuite { get; set; }
        void Run(int startIteration, int endIteration, int step = 1);
        void Run();
    }
}
