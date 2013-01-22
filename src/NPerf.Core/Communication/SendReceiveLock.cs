namespace NPerf.Core.Communication
{
    using System;
    using System.Threading;

    internal sealed class SendReceiveLock : IDisposable
    {
        private Semaphore empty;
        private Semaphore full;
        private bool disposed;

        public SendReceiveLock(string fullName, string emptyName, int size)
        {
            this.empty = NamedSemaphore.OpenOrCreate(emptyName, size, size);
            this.full = NamedSemaphore.OpenOrCreate(fullName, 0, size);
        }

        public void Send(Action sendAction)
        {
            this.empty.WaitOne();
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

        public object Receive(Func<object> receiveFunc)
        {
            object result;
            this.full.WaitOne();
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
