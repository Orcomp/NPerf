namespace NPerf.Framework.Laboratory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IPerfTestRunner
    {
        void Run(int startIteration, int endIteration, int step = 1);
        void Run();
    }
}
