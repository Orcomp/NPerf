using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPerf.Lab.Threading
{
    internal abstract class MultiRunnable : IRunnable
    {
        private IRunnable[] runnables;
        protected bool running = false;
        private bool disposed = false;

        protected void SetRunnables(IRunnable[] runnables)
        {
            this.runnables = runnables;
        }

        public virtual void Start()
        {
            if (!running && runnables != null)
            {
                running = true;
                for (int i = 0; i < runnables.Length; i++)
                    runnables[i].Start();
            }
        }

        public virtual void Stop()
        {
            if (running && runnables != null)
            {
                running = false;
                for (int i = 0; i < runnables.Length; i++)
                    runnables[i].Stop();
            }
        }

        public virtual void ForceAbort()
        {
            if (runnables != null)
            {
                running = false;
                for (int i = 0; i < runnables.Length; i++)
                    runnables[i].ForceAbort();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    for (int i = 0; i < runnables.Length; i++)
                        runnables[i].Dispose();
                }

                runnables = null;

                disposed = true;
            }
        }

        ~MultiRunnable()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
