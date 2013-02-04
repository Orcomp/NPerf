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

        public override int GetHashCode()
        {
            return TestId.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var testInfo = obj as TestInfo;
            if (testInfo == null)
            {
                return false;
            }

            return this.TestId.Equals(testInfo.TestId);
        }
    }
}
