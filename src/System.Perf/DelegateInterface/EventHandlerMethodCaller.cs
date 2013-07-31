using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Perf.DelegateInterface
{
    public class EventHandlerMethodCaller : IMethodCaller
    {
        int n = 0;
        event EventHandler d = null;

        public void SetMethod(IMethodCall mc, int n)
        {
            this.n = n;
            this.d += new EventHandler(mc.Call);
        }
        public void CallMethod()
        {
            for (int i = 0; i < n; ++i)
                d(this, new EventArgs());
        }
    }
}
