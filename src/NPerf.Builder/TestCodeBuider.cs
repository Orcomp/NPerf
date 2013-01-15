using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPerf.Framework;

namespace NPerf.Builder
{
    public class TestCodeBuider : IPerfTestInfo
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsIgnore { get; set; }

        public string IgnoreMessage { get; set; }

        public override string ToString()
        {
            return string.Format("new DynamicTest({0}, {1}, {2})",
                this.Name,
                this.Description,
                this.IsIgnore ? this.IgnoreMessage : ("this.testedObject." + this.Name));
        }
    }
}
