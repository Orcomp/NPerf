namespace NPerf.Experiment
{
    internal class SartParameters
    {
        public SartParameters()
        { 
        }

        public SartParameters(string[] args)
        {
            var arguments = args.ConvertToArguments();
            this.ChannelName = arguments.ExtractValue("channelName");
            this.SuiteAssembly = arguments.ExtractValue("suiteLib");
            this.SuiteType = arguments.ExtractValue("suiteType");
            this.SubjectAssembly = arguments.ExtractValue("subjectAssm");
            this.SubjectType = arguments.ExtractValue("subjecType");
            this.TestMethod = arguments.ExtractValue("testMethod");
            this.Start = arguments.ExtractValue("start");
            this.Step = arguments.ExtractValue("step");
            this.End = arguments.ExtractValue("end");
        }

        public string ChannelName { get; set; }

        public string SuiteAssembly { get; set; }

        public string SuiteType { get; set; }

        public string SubjectAssembly { get; set; }

        public string SubjectType { get; set; }

        public string TestMethod { get; set; }

        public string Start { get; set; }

        public string Step { get; set; }

        public string End { get; set; }
    }
}
