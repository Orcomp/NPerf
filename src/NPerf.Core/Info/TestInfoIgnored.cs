namespace NPerf.Core.Info
{
    using System;

    public class TestInfoIgnored : TestInfo
    {

        public string IgnoreMessage { get; set; }        

        public override int GetHashCode()
        {
            return TestId.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var testInfo = obj as TestInfoIgnored;
            if (testInfo == null)
            {
                return false;
            }

            return this.TestId.Equals(testInfo.TestId);
        }
    }
}
