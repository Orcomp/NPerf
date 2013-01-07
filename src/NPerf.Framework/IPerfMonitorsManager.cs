namespace NPerf.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IPerfMonitorsManager
    {
        IEnumerable<IPerfMonitor> Monitors { get; }

        IDisposable Observe();
    }
}
