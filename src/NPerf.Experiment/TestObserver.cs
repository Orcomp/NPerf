namespace NPerf.Experiment
{
    using System;
    using NPerf.Core.Communication;
    using NPerf.Core.PerfTestResults;

    internal class TestObserver : IObserver<TestResult>, IDisposable
    {
        private ProcessMailBox mailBox;

        private bool disposed;

        public TestObserver(string boxName)
        {
            this.mailBox = new ProcessMailBox(boxName);
        }

        public void OnCompleted()
        {
            this.mailBox.Content = TestResultFactory.Instance.Comleted();
        }

        public void OnError(Exception error)
        {
            this.mailBox.Content = TestResultFactory.Instance.FatalError(error);
        }

        public void OnNext(TestResult value)
        {
            this.mailBox.Content = value;
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
