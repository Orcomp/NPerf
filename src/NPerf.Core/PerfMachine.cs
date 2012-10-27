
namespace NPerf.Core
{
	using System;
	using System.Xml;
	using System.Xml.Serialization;
	using System.Management;	
	
	/// <summary>
	/// TODO - Add class summary
	/// </summary>
	[Serializable]
	[XmlRoot("machine")]
	public class PerfMachine
	{
		private string cpu = null;
		private long cpuFreq = 0;
		private long ram = 0;
		
		public PerfMachine()
		{}		
		
		/// <summary>
		/// Retreives system information about the machine.
		/// </summary>
		/// <returns>A <see cref="PerfMachine"/> object describing
		/// the physical configuration of the machine.
		/// </returns>
		/// <remarks>
		/// This method queries the register for physical information on the 
		/// system.
		/// </remarks>
		public static PerfMachine GetCurrent()
		{
			lock(typeof(PerfMachine))
			{
				PerfMachine machine = new PerfMachine();
			
				ManagementObjectSearcher query = new ManagementObjectSearcher("SELECT * From Win32_ComputerSystem");
				foreach(ManagementObject obj in query.Get())
				{
					machine.Ram =long.Parse(obj["TotalPhysicalMemory"].ToString());				
					break;				
				}
			
				query = new ManagementObjectSearcher("SELECT * From Win32_Processor");
				foreach(ManagementObject obj in query.Get())
				{
					machine.Cpu =(string)obj["Name"];				
					machine.CpuHz =long.Parse(obj["CurrentClockSpeed"].ToString());				
					break;				
				}
			
				return machine;
			}
		}
		
		[XmlAttribute("ram")]
		public long Ram
		{
			get
			{
				return this.ram;
			}
			set
			{
				this.ram = value;
			}
		}
		
		[XmlIgnore]
		public double RamMb
		{
			get
			{
				return Ram/(1024*1024);
			}
		}
		
		[XmlAttribute("cpu")]
		public string Cpu
		{
			get
			{
				return this.cpu;
			}
			set
			{
				this.cpu = value;
			}
		}
		
		[XmlAttribute("cpu-freq")]
		public long CpuHz
		{
			get
			{
				return this.cpuFreq;
			}
			set
			{
				this.cpuFreq = value;
			}
		}		
		
		[XmlIgnore]
		public double CpuMHz
		{
			get
			{
				return CpuHz/1.0e6;
			}
		}
	}
}
