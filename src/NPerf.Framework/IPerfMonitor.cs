namespace NPerf.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IPerfMonitor
    {
        void Start();

        void Stop();
    }
}
