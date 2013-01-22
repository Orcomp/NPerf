namespace NPerf.Core
{
    using System;

    [Serializable]
    public class ExpError
    {
        public ExpError()
        {
        }

        public ExpError(Exception ex)
        {
            this.Data = new PerfFailedResult(ex);
        }

        public PerfFailedResult Data { get; set; }
    }
}
