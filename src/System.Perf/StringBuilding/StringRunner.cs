using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Perf.StringBuilding
{
    public class StringRunner : IStringRunner
    {
        private String sb;
        public StringRunner()
        {
            this.sb = "";
        }

        public void Concat(string s)
        {
            sb += s;
        }

        public void ConcatFormat(string format, params string[] s)
        {
            sb += String.Format(format, s);
        }
    }	
}
