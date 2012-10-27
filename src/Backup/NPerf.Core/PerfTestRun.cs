using System;
using System.Xml;
using System.Xml.Serialization;

namespace NPerf.Core
{
	using NPerf.Core.Collections;
	
	[XmlRoot("run")]
	public class PerfTestRun
	{
		private double value;
		private PerfResultCollection results;
		private PerfFailedResultCollection failedResults;
		
		
		public PerfTestRun()
		{
			this.value = 0;
			this.results = new PerfResultCollection();
			this.failedResults = new PerfFailedResultCollection();
		}
		
		/// <summary>
		/// </summary>
		public PerfTestRun(double value)
		{
			this.value = value;
			this.results = new PerfResultCollection();
			this.failedResults = new PerfFailedResultCollection();
		}
		
		[XmlAttribute("value")]
		public double Value
		{
			get			
			{
				return this.value;				
			}
			set
			{
				this.value=value;
			}
		}
		
		[XmlArrayItem(ElementName="result",Type=typeof(PerfResult))]
		[XmlArray(ElementName="results")]			
		public PerfResultCollection Results
		{
			get
			{
				return this.results;
			}
			set
			{
				this.results = value;
			}
		}
		
		[XmlArrayItem(ElementName="failed-result",Type=typeof(PerfFailedResult))]
		[XmlArray(ElementName="failed-results")]			
		public PerfFailedResultCollection FailedResults
		{
			get
			{
				return this.failedResults;
			}
			set
			{
				this.failedResults = value;
			}
		}

	}
}
