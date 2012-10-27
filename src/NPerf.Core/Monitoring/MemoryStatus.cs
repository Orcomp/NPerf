using System;
using System.Diagnostics;
using System.Xml;
using System.Xml.Serialization;

namespace NPerf.Core.Monitoring
{
	/// <summary>
	/// Summary description for MemoryStatus.
	/// </summary>
	public class MemoryStatus
	{
		private long workingSet;
		private long peakWorkingSet;
		private long totalmemory;

		public MemoryStatus(Process p)
		{
			this.workingSet = p.WorkingSet;
			this.peakWorkingSet = p.PeakWorkingSet;
			this.totalmemory = GC.GetTotalMemory(false);
		}

		public long WorkingSet
		{
			get
			{
				return this.workingSet;
			}
		}

		public long PeakWorkingSet
		{
			get
			{
				return this.peakWorkingSet;
			}
		}
		
		public long TotalMemory
		{
			get
			{
				return this.totalmemory;
			}
		}
	}
}
