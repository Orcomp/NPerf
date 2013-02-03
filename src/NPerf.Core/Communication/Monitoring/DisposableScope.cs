namespace NPerf.Core.Monitoring
{
    using System;

    public class DisposableScope : IDisposable
    {
        private readonly Action end;

        public DisposableScope()
        { 
        }

        public DisposableScope(Action start, Action end)
        {
            this.end = end;
            start();
        }

        public void Dispose()
        {
            if (end != null)
            {
                this.end();
            }
        }
    }
}
