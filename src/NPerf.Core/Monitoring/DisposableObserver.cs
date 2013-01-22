using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPerf.Core.Monitoring
{
    public class DisposableObserver : IDisposable
    {
        private readonly Action end;

        public DisposableObserver()
        { 
        }

        public DisposableObserver(Action start, Action end)
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
