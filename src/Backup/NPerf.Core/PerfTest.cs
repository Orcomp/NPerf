using System;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;

namespace NPerf.Core
{
	using NPerf.Core.Collections;
	using NPerf.Framework;

	/// <summary>
	/// Summary description for PerfTest.
	/// </summary>
	[Serializable]
	[XmlRoot("test")]
	public class PerfTest
	{
		private bool ignored;
		private string ignoredMessage;
		private string name;
		private PerfTestRunCollection runs;

		public PerfTest()
		{
			this.name = null;
			this.runs = new PerfTestRunCollection();
			this.ignored = false;
			this.ignoredMessage = null;
		}

		public PerfTest(MethodInfo mi)
		{
			if (mi==null)
				throw new ArgumentNullException("mi");
			
			this.name = mi.Name;
			this.runs = new PerfTestRunCollection();
			
			this.ignored = TypeHelper.HasCustomAttribute(mi, typeof(PerfIgnoreAttribute));
			if (this.ignored)
			{
				PerfIgnoreAttribute attr = (PerfIgnoreAttribute)
					TypeHelper.GetFirstCustomAttribute(mi,typeof(PerfIgnoreAttribute));
				this.ignoredMessage=attr.Message;
			}
			else
				this.ignoredMessage = null;
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
		
		[XmlAttribute("ignored")]
		public bool IsIgnored
		{
			get
			{
				return this.ignored;
			}
			set
			{
				this.ignored = value;
			}
		}
		
		[XmlAttribute("ignored-message")]
		public string IgnoredMessage		
		{
			get
			{
				return this.ignoredMessage;
			}
			set
			{
				this.ignoredMessage = value;
			}			
		}
		
		[XmlArrayItem(ElementName="run",Type=typeof(PerfTestRun))]
		[XmlArray(ElementName="runs")]			
		public PerfTestRunCollection Runs
		{
			get
			{
				return this.runs;
			}
			set
			{
				this.runs = value;
			}
		}		
	}
}
