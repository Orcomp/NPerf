namespace NPerf.Experiment
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using NPerf.Framework;

    internal class Monitoring : IDisposable
    {
        private IPerfTestSuite suite;

        private bool disposed = false;


        public Monitoring(IPerfTestSuite suite, int iteration)
        {
            this.suite = suite;

            foreach (var monitor in this.suite.Monitors)
            {
                monitor.Start(iteration);
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="Monitoring"/> class. 
        /// </summary>
        ~Monitoring()
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
                foreach (var monitor in this.suite.Monitors)
                {
                    monitor.Stop();
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
