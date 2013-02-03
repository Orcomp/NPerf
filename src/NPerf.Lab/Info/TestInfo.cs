namespace NPerf.Lab.Info
{
    using System;
    using NPerf.Framework.Interfaces;

    public class TestInfo : IPerfTestInfo
    {
        internal TestInfo()
        {
        }

        public Guid TestId { get; set; }

        public string TestMethodName { get; set; }

        public string TestDescription { get; set; }

        public TestSuiteInfo Suite { get; set; }
    }
}
