namespace NPerf.Core
{
    using System;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;
    using System.Collections.Generic;

    /// <summary>
    /// Summary description for PerfTestSuite.
    /// </summary>
    [Serializable]
    [XmlRoot("test-suite")]
    public class PerfTestSuite
    {
        public PerfTestSuite()
        {
            this.TimeStamp = DateTime.Now;
            this.Tests = new List<PerfTest>();
        }

        public PerfTestSuite(Type testerType, string description, string featureDescription)
        {
            if (testerType == null)
            {
                throw new ArgumentNullException("testerType");
            }

            this.TimeStamp = DateTime.Now;
            this.Name = testerType.Name;
            this.Description = description;
            this.FeatureDescription = featureDescription;
            this.Tests = new List<PerfTest>();
            this.Machine = PerfMachine.GetCurrent();
            this.Os = PerfOs.GetCurrent();
        }


        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("description")]
        public string Description { get; set; }


        [XmlAttribute("feature-description")]
        public string FeatureDescription { get; set; }

        [XmlAttribute("time-stamp")]
        public DateTime TimeStamp { get; set; }

        [XmlElement("machine", IsNullable = false)]
        public PerfMachine Machine { get; set; }

        [XmlElement("os", IsNullable = false)]
        public PerfOs Os { get; set; }

        [XmlArrayItem(ElementName = "test", Type = typeof(PerfTest))]
        [XmlArray(ElementName = "tests")]
        public IList<PerfTest> Tests { get; set; }

        public override String ToString()
        {
            var sw = new StringWriter();
            this.ToXml(sw);

            return sw.ToString();
        }

        public void ToXml(TextWriter writer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }

            var xmlWriter = new XmlTextWriter(writer) { Formatting = Formatting.Indented };
            Serializer.Serialize(xmlWriter, this);
        }

        [XmlIgnore]
        public static XmlSerializer Serializer
        {
            get
            {
                return new XmlSerializer(typeof(PerfTestSuite), "http://www.parser.info");
            }
        }
    }
}
