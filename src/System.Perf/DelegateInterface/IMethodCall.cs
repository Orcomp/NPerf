using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Perf.DelegateInterface
{
    public interface IMethodCall
    {
        void Call();
        void Call(object sender, EventArgs args);
    }
}
