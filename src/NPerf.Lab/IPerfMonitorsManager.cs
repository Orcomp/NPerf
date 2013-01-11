namespace NPerf.Lab
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using NPerf.Framework;

    public interface IPerfMonitorsManager
    {
        IEnumerable<IPerfMonitor> Monitors { get; }

        IDisposable Observe();
    }
}
