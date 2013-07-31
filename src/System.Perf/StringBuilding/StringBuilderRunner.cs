using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Perf.StringBuilding
{
    public class StringBuilderRunner : IStringRunner
    {
        private StringBuilder sb;
        public StringBuilderRunner()
        {
            this.sb = new StringBuilder();
        }

        public void Concat(string s)
        {
            sb.Append(s);
        }

        public void ConcatFormat(string format, params string[] s)
        {
            sb.AppendFormat(format, s);
        }
    }
}
