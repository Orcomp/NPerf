using System;
using System.Diagnostics;

namespace NPerf.Core.Monitoring
{


	/// <summary>
	/// Summary description for MemoryTracker.
	/// </summary>
	public class MemoryMonitor
	{
		private MemoryStatus startStatus = null;
		private MemoryStatus endStatus = null;

		public MemoryMonitor()
		{}

		public MemoryStatus StartStatus
		{
			get
			{
				return this.startStatus;
			}
		}

		public MemoryStatus EndStatus
		{
			get
			{
				return this.endStatus;
			}
		}

		public long Usage
		{
			get
			{
				return EndStatus.TotalMemory - StartStatus.TotalMemory; 
			}
		}

		public void Start()
		{
			this.startStatus = new MemoryStatus(Process.GetCurrentProcess());
			this.endStatus = null;
		}

		public void Stop()
		{
			this.endStatus = new MemoryStatus(Process.GetCurrentProcess());
		}
	}
}
