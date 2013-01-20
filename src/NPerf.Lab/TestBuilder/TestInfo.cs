using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPerf.Framework;
using NPerf.Framework.Interfaces;

namespace NPerf.Lab.TestBuilder
{
    public class TestInfo : IPerfTestInfo
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsIgnore { get; set; }

        public string IgnoreMessage { get; set; }
    }
}
