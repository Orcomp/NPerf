namespace NPerf.Core.Communication
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed class SendReceiveLock : IDisposable
    {
        private Semaphore empty;

        private Semaphore full;

        private bool disposed;

        public SendReceiveLock(string fullName, string emptyName)
        {
            this.empty = NamedSemaphore.OpenOrCreate(emptyName, 1, 1);
            this.full = NamedSemaphore.OpenOrCreate(fullName, 0, 1);
        }

        public void Send(Action sendAction, TimeSpan timeout)
        {
            if (this.empty.WaitOne(timeout))
            {
                try
                {
                    sendAction();
                }
                catch
                {
                    this.empty.Release();
                    throw;
                }
                this.full.Release();
            }            
        }

        public object Receive(Func<object> receiveFunc, TimeSpan timeout)
        {
            object result = null;
            if (this.full.WaitOne(timeout))
            {
                try
                {
                    result = receiveFunc();
                }
                catch
                {
                    this.full.Release();
                    throw;
                }

                this.empty.Release();
            }
            return result;
        }

        private void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                this.empty.Dispose();
                this.full.Dispose();
            }

            this.empty = null;
            this.full = null;

            this.disposed = true;
        }

        ~SendReceiveLock()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
