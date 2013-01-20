namespace NPerf.Framework.Interfaces
{
    /// <summary>
    /// The PerformanceMonitor interface.
    /// </summary>
    public interface IPerfMonitor
    {
        /// <summary>
        /// Starts monitoring.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops monitoring.
        /// </summary>
        void Stop();
    }
}
