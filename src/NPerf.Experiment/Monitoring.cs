namespace NPerf.Experiment
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using NPerf.Framework;

    internal class Monitoring : IDisposable
    {
        private IPerfFixture tool;

        private bool disposed = false;


        public Monitoring(IPerfFixture tool, int iteration)
        {
            this.tool = tool;

            foreach (var monitor in this.tool.Monitors)
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
                foreach (var monitor in this.tool.Monitors)
                {
                    monitor.Stop();
                }
            }

            this.tool = null;
            this.disposed = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
        }              
    }
}
