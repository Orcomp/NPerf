using System;
using System.Xml.Serialization;

namespace NPerf.Core
{
	/// <summary>
	/// Class describing benchmark results.
	/// </summary>
	[Serializable]
	[XmlRoot("result")]
	public class PerfResult
	{
		private string testedType = null;
		private double duration = 0;
		private long memoryUsage = 0;
		
		public PerfResult()
		{}

		/// <summary>
		/// Default constructor
		/// </summary>
		public PerfResult(Type testedType, double duration, long memoryUsage)
		{
			if (testedType==null)
				throw new ArgumentNullException("testedType");
			this.testedType = testedType.Name;
			this.duration = duration;
			this.memoryUsage = memoryUsage;
		}
		
		[XmlAttribute("tested-type")]
		public string TestedType
		{
			get
			{
				return this.testedType;
			}
			set
			{
				this.testedType = value;
			}
		}

		/// <summary>
		/// Gets the test duration
		/// </summary>
		/// <value>
		/// Test duration
		/// </value>
		[XmlAttribute("duration")]
		public double Duration
		{
			get
			{
				return this.duration;				
			}
			set
			{
				this.duration = value;
			}
		}
		
		[XmlAttribute("memory-usage")]
		public long MemoryUsage
		{
			get
			{
				return this.memoryUsage;				
			}
			set
			{
				this.memoryUsage = value;
			}
		}
		
		[XmlIgnore]
		public double MemoryUsageMb
		{
			get
			{
				return (double)this.memoryUsage/(1024*1024);
			}
		}
	}
}
