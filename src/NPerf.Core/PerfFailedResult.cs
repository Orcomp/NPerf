namespace NPerf.Core
{
    using System;

    [Serializable]
    public class PerfFailedResult
    {
        public PerfFailedResult()
        {
        }

        /// <summary>
        /// Default constructor - initializes all fields to default values
        /// </summary>
        public PerfFailedResult(Exception ex)
        {
            if (ex == null)
            {
                throw new ArgumentNullException("ex");
            }

            var iex = ex;

            if (ex.InnerException != null)
            {
                iex = ex.InnerException;
            }

            this.Descriptor = RunDescriptor.Instance.Value;

            this.ExceptionType = iex.GetType().Name;
            this.Message = iex.Message;
            this.Source = iex.Source;
            this.FullMessage = iex.ToString();
        }
        
        public double Descriptor { get; set; }

        public string ExceptionType { get; set; }

        public string Message { get; set; }

        public string Source { get; set; }

        public string FullMessage { get; set; }
    }
}
