using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPerf.Core
{
    using NPerf.Framework.Interfaces;

    public class TestInfo : IPerfTestInfo
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public override bool Equals(object obj)
        {
            var testInfo = obj as TestInfo;
            if (testInfo == null)
            {
                return false;
            }

            return (this.Name ?? string.Empty).Equals(testInfo.Name)
                   && (this.Description ?? string.Empty).Equals(testInfo.Description);
        }

        public override int GetHashCode()
        {
            return string.IsNullOrEmpty(this.Name) ? 0 : this.Name.Length;
        }
    }
}
