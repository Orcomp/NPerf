namespace NPerf.Experiment
{
    using System.Linq;

    internal class StartParameters
    {
        public StartParameters()
        { 
        }

        public StartParameters(string[] args)
        {
            var arguments = args.ConvertToArguments();
            this.ChannelName = arguments.ExtractValue("channelName");
            this.SuiteAssembly = arguments.ExtractValue("suiteLib");
            this.SuiteType = arguments.ExtractValue("suiteType");
            this.TesterAssembly = arguments.ExtractValue("testerAssm");
            this.SubjectAssembly = arguments.ExtractValue("subjectAssm");
            this.SubjectType = arguments.ExtractValue("subjecType");
            this.TestMethod = arguments.ExtractValue("testMethod");
            this.Start = arguments.ExtractValue("start");
            this.Step = arguments.ExtractValue("step");
            this.End = arguments.ExtractValue("end");
            this.IgnoreFirstRunDueToJITting = arguments.Count(a => a.Name.Equals("ignoreFirstRun")) > 0;
            this.TriggerGCBeforeEachTest = arguments.Count(a => a.Name.Equals("triggerGC")) > 0;
        }

        public string ChannelName { get; set; }

        public string SuiteAssembly { get; set; }

        public string SuiteType { get; set; }

        public string TesterAssembly { get; set; }

        public string SubjectAssembly { get; set; }

        public string SubjectType { get; set; }

        public string TestMethod { get; set; }

        public string Start { get; set; }

        public string Step { get; set; }

        public string End { get; set; }

        public bool IgnoreFirstRunDueToJITting { get; set; }

        public bool TriggerGCBeforeEachTest { get; set; }
    }
}
