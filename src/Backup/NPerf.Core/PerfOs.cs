
namespace NPerf.Core
{
	using System;
	using System.Xml;
	using System.Xml.Serialization;
	
	
	/// <summary>
	/// TODO - Add class summary
	/// </summary>
	/// <remarks>
	/// 	created by - dehalleux
	/// 	created on - 21/01/2004 15:06:22
	/// </remarks>
	[Serializable]
	[XmlRoot("os")]
	public class PerfOs
	{
		private string name;
		private string version;
				
		public PerfOs()
		{}

		/// <summary>
		/// Retreives the current machine operating system information.
		/// </summary>
		/// <returns><see cref="PerfOs"/> object describing the current
		/// operation system</returns>
		public static PerfOs GetCurrent()
		{
			lock(typeof(PerfOs))
			{
				PerfOs cur = new PerfOs();
				cur.Name = Environment.OSVersion.Platform.ToString();
				cur.Version = Environment.Version.ToString();

				return cur;
			}
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
		
		[XmlAttribute("version")]
		public string Version
		{
			get
			{
				return this.version;
			}
			set
			{
				this.version = value;
			}
		}		
	}
}
