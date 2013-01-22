using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace System.Perf.StringBuilding
{
    public class StringWriterRunner : IStringRunner
    {
        private StringWriter sw;
        public StringWriterRunner()
        {
            this.sw = new StringWriter();
        }

        public void Concat(string s)
        {
            sw.Write(s);
        }

        public void ConcatFormat(string format, params string[] s)
        {
            sw.Write(format, s);
        }
    }
}
