namespace NPerf.Lab.Info
{
    using System;
    using NPerf.Framework.Interfaces;

    public class TestSuiteInfo : IPerfTestSuiteInfo
    {
        internal TestSuiteInfo()
        {
        }

        public Type TesterType { get; set; }

        public Type TestedType { get; set; }

        public Type TestedAbstraction { get; set; }

        public int DefaultTestCount { get; set; }

        public string TestSuiteDescription { get; set; }

        public string FeatureDescription { get; set; }

        public TestInfo[] Tests { get; set; }

        public TestInfoIgnored[] IgnoredTests { get; set; }
    }
}
