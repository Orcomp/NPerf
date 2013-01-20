namespace NPerf.Experiment
{
    using System;
    using NPerf.Framework.Interfaces;

    internal class PerfObserver : IDisposable
    {
        private IPerfTestSuite suite;

        private bool disposed = false;


        public PerfObserver(IPerfTestSuite suite)
        {
            this.suite = suite;

            if (this.suite.Monitors != null)
                foreach (var monitor in this.suite.Monitors)
                {
                    monitor.Start();
                }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="PerfObserver"/> class. 
        /// </summary>
        ~PerfObserver()
        {
            this.Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                if (this.suite.Monitors != null)
                {
                    foreach (var monitor in this.suite.Monitors)
                    {
                        monitor.Stop();
                    }
                }
            }

            this.suite = null;
            this.disposed = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
        }              
    }
}
