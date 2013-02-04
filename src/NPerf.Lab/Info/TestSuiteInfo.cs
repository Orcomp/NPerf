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

        public override int GetHashCode()
        {

            return (TesterType == null ? 0 : TesterType.GetHashCode()) +
                (TestedType == null ? 0 : TestedType.GetHashCode()) +
                (TestedAbstraction == null ? 0 : TestedAbstraction.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            var info = obj as TestSuiteInfo;
            if (info == null)
            {
                return false;
            }

            return Equals(TesterType, info.TesterType) && Equals(TestedType, info.TestedType) 
                && Equals(TestedAbstraction, info.TestedAbstraction);
        }
    }
}
