using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Perf.DelegateInterface
{
    public class MethodCall : IMethodCall
    {
        public void Call()
        { }
        public void Call(object sender, EventArgs args)
        { }
    }
}
