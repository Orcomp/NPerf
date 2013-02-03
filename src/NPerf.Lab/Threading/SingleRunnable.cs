namespace NPerf.Lab.Threading
{
    using System;
    using System.Threading;

    /// <summary>
    /// SingleRunnable provides simple management of a threaded blocking loop implementations.
    /// </summary>
    internal abstract class SingleRunnable : IRunnable
    {
        private Thread thread;
        /// <summary>Use this variable in your infinite while loop condition.</summary>
        protected bool running = false;
        private bool aborted;
        private readonly bool interruptOnStop;
        private bool disposed;

        /// <summary>Initialize the runnable base class.</summary>
        //protected SingleRunnable() : this(true,false,false) {}
        /// <summary>Initialize the runnable base class.</summary>
        /// <param name="interruptOnStop">If true, an interrupt is fired on <see cref="IRunnable.Stop"/></param>
        /// <param name="autoStart">If true, the runnably is started automatically.</param>
        //protected SingleRunnable(bool interruptOnStop, bool autoStart) : this(interruptOnStop,autoStart,false) {}
        /// <summary>Initialize the runnable base class.</summary>
        /// <param name="interruptOnStop">If true, an interrupt is fired on <see cref="IRunnable.Stop"/></param>
        /// <param name="autoStart">If true, the runnably is started automatically.</param>
        /// <param name="waitOnStop">If true, <see cref="IRunnable.Stop"/> waits until the thread really finished.</param>
        protected SingleRunnable(bool interruptOnStop, bool autoStart)
        {
            this.interruptOnStop = interruptOnStop;
            if (autoStart)
                Start();
        }

        /// <summary>
        /// Override this method to implement your loop. Check for thread interrupts is recommended.
        /// </summary>
        protected abstract void Run();

        private void ThreadRun()
        {
            try { Run(); }
            catch (System.Threading.ThreadInterruptedException) { }
        }

        /// <summary>
        /// Start the runnable.
        /// </summary>
        public virtual void Start()
        {
            if (aborted)
                throw new InvalidOperationException();
            if (thread == null)
            {
                running = true;
                thread = new Thread(new ThreadStart(this.ThreadRun));
                thread.Start();
            }
        }

        /// <summary>
        /// Stop the runnable. This method tells the thread to finish it's work and waits until the thread really finishes if sync is enabled.
        /// </summary>
        public virtual void Stop()
        {
            if (aborted)
                throw new InvalidOperationException();
            if (thread != null)
            {
                running = false;
                if (interruptOnStop)
                    thread.Interrupt();
                thread.Join();
            }
        }

        /// <summary>
        /// Stop the runnable. This method forces the threads to abort and propably leaves data in an invalid state. It won't wait until the thread really aborts.
        /// </summary>
        public virtual void ForceAbort()
        {
            if (thread != null)
            {
                aborted = true;
                running = false;
                thread.Abort();
                thread = null;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    Stop();
                }

                thread = null;

                disposed = true;
            }
        }

        ~SingleRunnable()
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
