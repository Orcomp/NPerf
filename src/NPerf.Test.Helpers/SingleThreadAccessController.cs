namespace NPerf.Test.Helpers
{
    using System;
    using System.Threading;

    public class SingleThreadAccessController : IDisposable
    {
        private static int threadId = -1;

        public SingleThreadAccessController()
        {
            if (threadId != -1)
            {
                throw new InvalidOperationException("Access from several threads");
            }

            threadId = Thread.CurrentThread.ManagedThreadId;
        }

        public void Dispose()
        {
            threadId = -1;
        }
    }
}
