using System;

namespace NPerf.Framework
{
	/// <summary>
	/// Defines a benchmark test method.
	/// </summary>
	/// <include file='NPerf.Framework.Doc.xml' path='doc/remarkss/remarks[@name="PerfTestAttribute"]'/>
	/// <include file='NPerf.Framework.Doc.xml' path='doc/examples/example[@name="PerfTestAttribute"]'/>
	[AttributeUsage(AttributeTargets.Method,AllowMultiple=false)]	
	public class PerfTestAttribute : Attribute
	{
		private string description = null;
		
		/// <summary>
		/// Empty constructor
		/// </summary>
		public PerfTestAttribute()
		{}
		
		/// <summary>
		/// Constructor with test descrition
		/// </summary>
		/// <remarks>
		/// Constructs the attribute with <paramref name="description"/>.
		/// </remarks>
		/// <exception cref="ArgumentNullException">description is a null reference</exception>
		public PerfTestAttribute(string description)
		{
			if (description==null)
				throw new ArgumentNullException("description");
			this.description = description;
		}
		
		/// <summary>Gets the Test description string</summary>
		/// <value>Test description</value>
		public string Description
		{
			get
			{
				return this.description;
			}
		}
	}
}
