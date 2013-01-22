using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Perf.DelegateInterface
{
    public class DelegateMethodCaller : IMethodCaller
    {
        int n = 0;
        event Action d = null;

        public void SetMethod(IMethodCall mc, int n)
        {
            this.n = n;
            this.d += new Action(mc.Call);
        }
        public void CallMethod()
        {
            for (int i = 0; i < n; ++i)
                d();
        }
    }
}
