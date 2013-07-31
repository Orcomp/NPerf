namespace NPerf.Core.Communication.Monitoring
{
    using System;

    /// <summary>
    /// The PerformanceMonitor interface.
    /// </summary>
    public interface IPerfMonitor<T>
    {
        IDisposable Observe();

        T Value { get; }
    }
}
