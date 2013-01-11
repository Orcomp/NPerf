namespace NPerf.Lab
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using NPerf.Framework;

    public interface IPerfTestFactory
    {
        IPerfTestRunner CreateTestRunner(Assembly nPrefFixture, IPerfMonitor[] monitors = null, params Assembly[] libsToTest);      
    }
}
