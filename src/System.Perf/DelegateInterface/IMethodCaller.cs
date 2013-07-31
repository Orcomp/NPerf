using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Perf.DelegateInterface
{
    public interface IMethodCaller
    {
        void SetMethod(IMethodCall m, int n);
        void CallMethod();
    }
}
