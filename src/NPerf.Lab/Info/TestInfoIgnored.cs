namespace NPerf.Lab.Info
{
    using System;
    using NPerf.Framework.Interfaces;

    public class TestInfoIgnored : IPerfTestInfo
    {
        internal TestInfoIgnored()
        {
        }

        public Guid TestId { get; set; }

        public string TestMethodName { get; set; }

        public string TestDescription { get; set; }

        public string IgnoreMessage { get; set; }
        
        public TestSuiteInfo Suite { get; set; }
    }
}
