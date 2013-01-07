namespace NPerf.Monitors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using NPerf.Framework;

    public class Monitor<T> : IPerfMonitor
    {
        // TODO: send value returned from fixture.RunDescriptor() to the monitor (if needed) On start or on stop
        public Monitor(IFixture<T> fixture)
        {
            
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
