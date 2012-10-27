using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace NPerf.Core
{
	using NPerf.Core.Collections;
	/// <summary>
	/// Summary description for PerfTestSuite.
	/// </summary>
	[Serializable]
	[XmlRoot("test-suite")]
	public class PerfTestSuite
	{
		private DateTime timeStamp;
		private string name = null;
		private string description = null;
		private string featureDescription = null;
		private PerfTestCollection tests;
		private PerfMachine machine = null;
		private PerfOs os = null;

		public PerfTestSuite()
		{
			this.timeStamp = DateTime.Now;
			this.tests = new PerfTestCollection();
		}

		public PerfTestSuite(Type testerType, string description, string featureDescription)
		{
			if (testerType==null)
				throw new ArgumentNullException("testerType");
			
			this.timeStamp = DateTime.Now;
			this.name = testerType.Name;
			this.description = description;
			this.featureDescription = featureDescription;
			this.tests = new PerfTestCollection();
			this.machine = PerfMachine.GetCurrent();
			this.os = PerfOs.GetCurrent();
		}

		
		[XmlAttribute("name")]
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		[XmlAttribute("description")]
		public string Description
		{
			get
			{
				return this.description;
			}
			set
			{
				this.description = value;
			}
		}


		[XmlAttribute("feature-description")]
		public string FeatureDescription
		{
			get
			{
				return this.featureDescription;
			}
			set
			{
				this.featureDescription = value;
			}
		}

		[XmlAttribute("time-stamp")]
		public DateTime TimeStamp
		{
			get
			{
				return this.timeStamp;
			}
			set
			{
				this.timeStamp =value;
			}
		}

		[XmlElement("machine", IsNullable=false)]
		public PerfMachine Machine
		{
			get
			{
				return this.machine;
			}
			set
			{
				this.machine = value;
			}
		}

		[XmlElement("os", IsNullable=false)]
		public PerfOs Os
		{
			get
			{
				return this.os;
			}
			set
			{
				this.os = value;
			}
		}

		[XmlArrayItem(ElementName="test",Type=typeof(PerfTest))]
		[XmlArray(ElementName="tests")]			
		public PerfTestCollection Tests
		{
			get
			{
				return this.tests;
			}
			set
			{
				this.tests = value;
			}
		}

		public override String ToString()
		{
			StringWriter sw = new StringWriter();
			ToXml(sw);
			
			return sw.ToString();
		}
		
		public void ToXml(TextWriter writer)
		{
			if (writer==null)
				throw new ArgumentNullException("writer");
			
			XmlTextWriter xmlWriter = new XmlTextWriter(writer);
			xmlWriter.Formatting = Formatting.Indented;
			Serializer.Serialize(xmlWriter,this);
		}

		[XmlIgnore]
		public static XmlSerializer Serializer
		{
			get
			{
				return new XmlSerializer(typeof(PerfTestSuite),"http://www.parser.info");
			}
		}
	}
}
