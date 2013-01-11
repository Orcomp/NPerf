namespace NPerf.Framework
{
    /// <summary>
    /// The PerformanceMonitor interface.
    /// </summary>
    public interface IPerfMonitor
    {
        /// <summary>
        /// Gets the name of performance monitor.
        /// </summary>
        string MonitorName { get; }

        /// <summary>
        /// Starts monitoring.
        /// </summary>
        /// <param name="iteration">
        /// The iteration of current test.
        /// </param>
        void Start(int iteration);

        /// <summary>
        /// Stops monitoring.
        /// </summary>
        void Stop();
    }
}
