namespace NPerf.Framework.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using NPerf.Framework;
    using NPerf.Framework.Interfaces;

    public interface IPerfTestFactory
    {
        IPerfTestRunner[] CreateTestRunner(Assembly nPrefFixture, IPerfMonitor[] monitors = null, params Assembly[] libsToTest);      
    }
}
