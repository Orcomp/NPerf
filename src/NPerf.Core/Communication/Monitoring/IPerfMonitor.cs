namespace NPerf.Core.Monitoring
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
