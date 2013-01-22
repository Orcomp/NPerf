namespace NPerf.Experiment
{
    using System;
    using NPerf.Core;
    using NPerf.Core.Communication;

    internal class TestObserver : IObserver<TestResult>, IDisposable
    {
        private ProcessMailBox mailBox;

        private bool disposed;

        public TestObserver(string boxName)
        {
            this.mailBox = new ProcessMailBox(boxName, 1024);
        }

        public void OnCompleted()
        {
            this.mailBox.Content = new ExpComplete();
        }

        public void OnError(Exception error)
        {
            this.mailBox.Content = new ExpError(error);
        }

        public void OnNext(TestResult value)
        {
            this.mailBox.Content = value.Data;
        }

        ~TestObserver()
        {
            this.Dispose(false);
        }

        private void Dispose(bool dispose)
        {
            if (this.disposed)
            {
                return;
            }

            if (dispose)
            {
                this.mailBox.Dispose();
            }

            this.mailBox = null;
            this.disposed = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
