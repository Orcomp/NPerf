using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Perf.DelegateInterface
{
    public class InterfaceMethodCall : IMethodCaller
    {
        int n = 0;
        IMethodCall mc = null;

        public void SetMethod(IMethodCall mc, int n)
        {
            this.n = n;
            this.mc = mc;
        }
        public void CallMethod()
        {
            for (int i = 0; i < n; ++i)
                mc.Call();
        }
    }
}
