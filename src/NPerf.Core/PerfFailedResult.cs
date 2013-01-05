namespace NPerf.Core
{
    using System;
    using System.Xml;
    using System.Xml.Serialization;

    [Serializable]
    [XmlRoot("failed-result")]
    public class PerfFailedResult
    {
        /// <summary>
        /// Default constructor - initializes all fields to default values
        /// </summary>
        public PerfFailedResult(Type testedType, Exception ex)
        {
            if (testedType == null)
            {
                throw new ArgumentNullException("testedType");
            }

            if (ex == null)
            {
                throw new ArgumentNullException("ex");
            }

            var iex = ex;

            if (ex.InnerException != null)
            {
                iex = ex.InnerException;
            }

            this.TestedType = testedType.Name;
            this.ExceptionType = iex.GetType().Name;
            this.Message = iex.Message;
            this.Source = iex.Source;
            this.FullMessage = iex.ToString();
        }

        [XmlAttribute("tested-type")]
        public string TestedType { get; set; }

        [XmlAttribute("exception-type")]
        public string ExceptionType { get; set; }

        [XmlElement("message")]
        public string Message { get; set; }

        [XmlElement("source")]
        public string Source { get; set; }

        [XmlElement("full-message")]
        public string FullMessage { get; set; }
    }
}
