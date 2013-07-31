namespace NPerf.Core.PerfTestResults
{
    using System;

    [Serializable]
    public class ExperimentError : PerfTestResult
    {
        public ExperimentError()
        {
        }

        public ExperimentError(Exception ex, double descriptor = -1)
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

            this.ExceptionType = iex.GetType().Name;
            this.Message = iex.Message;
            this.Source = iex.Source;
            this.FullMessage = iex.ToString();
            this.Descriptor = descriptor;
        }

        public string ExceptionType { get; set; }

        public string Message { get; set; }

        public string Source { get; set; }

        public string FullMessage { get; set; }

        public override bool Equals(object obj)
        {
            var experimentError = obj as ExperimentError;
            if (experimentError == null)
            {
                return false;
            }

            return string.Equals(this.ExceptionType, experimentError.ExceptionType)
                   && string.Equals(this.Message, experimentError.Message)
                   && string.Equals(this.Source, experimentError.Source)
                   && string.Equals(this.FullMessage, experimentError.FullMessage) && base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() + this.ExceptionType.Length;
        }
    }
}
