using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Perf.StringBuilding
{
    public interface IStringRunner
    {
        void Concat(string s);
        void ConcatFormat(string format, params string[] s);
    }
}
