using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPerf.Lab
{
    using System.ComponentModel;

    public class PerfTestConfiguration
    {
        [Category("Global Configuration")]
        [DisplayName("Ignore first test run")]
        [Description("Ignore first test run to avoid JIT compilation overhead.")]
        public bool IgnoreFirstRunDueToJITting { get; set; }

        [Category("Global Configuration")]
        [DisplayName("Trigger GC before each test")]
        [Description("Trigger garbage collection before each test run.")]
        public bool TriggerGCBeforeEachTest { get; set; }

        public PerfTestConfiguration()
        {
            
        }

        public PerfTestConfiguration(bool ignoreFirstRun, bool triggerGC)
        {
            this.IgnoreFirstRunDueToJITting = ignoreFirstRun;
            this.TriggerGCBeforeEachTest = triggerGC;
        }

        public PerfTestConfiguration Clone()
        {
            return new PerfTestConfiguration(this.IgnoreFirstRunDueToJITting, this.TriggerGCBeforeEachTest);
        }

        public void Copy(PerfTestConfiguration configuration)
        {
            this.IgnoreFirstRunDueToJITting = configuration.IgnoreFirstRunDueToJITting;
            this.TriggerGCBeforeEachTest = configuration.TriggerGCBeforeEachTest;
        }
    }
}
