using System;
using System.Xml;
using System.Xml.Serialization;


namespace NPerf.Core
{
	[Serializable]
	[XmlRoot("failed-result")]	
	public class PerfFailedResult
	{
		private string testedType;
		private string exceptionType;
		private string message;
		private string source;
		private string fullMessage;
		
		public PerfFailedResult()
		{
			this.testedType=null;
			this.exceptionType = null;
			this.message = null;
			this.source = null;
			this.fullMessage = null;
		}
		
		/// <summary>
		/// Default constructor - initializes all fields to default values
		/// </summary>
		public PerfFailedResult(Type testedType, Exception ex)
		{
			if (testedType==null)
				throw new ArgumentNullException("testedType");
			if (ex==null)
				throw new ArgumentNullException("ex");
			
			Exception iex = ex;
			
			if (ex.InnerException!=null)
				iex = ex.InnerException;
			this.testedType = testedType.Name;
			this.exceptionType = iex.GetType().Name;
			this.message = iex.Message;
			this.source = iex.Source;
			this.fullMessage = iex.ToString();
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

		[XmlAttribute("exception-type")]		
		public string ExceptionType
		{
			get
			{
				return this.exceptionType;
			}
			set
			{
				this.exceptionType = value;
			}
		}
		
		[XmlElement("message")]		
		public string Message
		{
			get
			{
				return this.message;
			}
			set
			{
				this.message = value;
			}
		}
		
		[XmlElement("source")]		
		public string Source
		{
			get
			{
				return this.source;
			}
			set
			{
				this.source = value;
			}
		}
		
		[XmlElement("full-message")]		
		public string FullMessage
		{
			get
			{
				return this.fullMessage;
			}
			set
			{
				this.FullMessage = value;
			}
		}
				
	}
}
