namespace NPerf.Core.PerfTestResults
{
    using System;

    [Serializable]
    public abstract class PerfTestResult
    {
        public Guid TestId { get; set; }

        public double Descriptor { get; set; }

        public override string ToString()
        {
            return string.Format("{0} d={1}", this.GetType().Name, Descriptor);
        }

        public override bool Equals(object obj)
        {
            var testResult = obj as PerfTestResult;
            return testResult != null && this.Descriptor.Equals(testResult.Descriptor);
        }

        public override int GetHashCode()
        {
            return (int)this.Descriptor;
        }
    }
}
