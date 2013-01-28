namespace NPerf.Experiment
{
    internal class SartParameters
    {
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

        public string ChannelName { get; private set; }

        public string SuiteAssembly { get; private set; }

        public string SuiteType { get; private set; }

        public string SubjectAssembly { get; private set; }

        public string SubjectType { get; private set; }

        public string TestMethod { get; private set; }

        public string Start { get; private set; }

        public string Step { get; private set; }

        public string End { get; private set; }
    }
}
