namespace NPerf.Core.Info
{
    using System;

    public class TestSuiteInfo
    {
        public Type TesterType { get; set; }

        public Type TestedAbstraction { get; set; }

        public int DefaultTestCount { get; set; }

        public string TestSuiteDescription { get; set; }

        public string FeatureDescription { get; set; }

        public TestInfo[] Tests { get; set; }

        public TestInfoIgnored[] IgnoredTests { get; set; }

        public string GetDescriptoMethodName { get; set; }

        public override int GetHashCode()
        {

            return (TesterType == null ? 0 : TesterType.GetHashCode()) +
                (TestedAbstraction == null ? 0 : TestedAbstraction.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            var info = obj as TestSuiteInfo;
            if (info == null)
            {
                return false;
            }

            return this.TesterType == info.TesterType
                && this.TestedAbstraction == info.TestedAbstraction;
        }
    }
}
