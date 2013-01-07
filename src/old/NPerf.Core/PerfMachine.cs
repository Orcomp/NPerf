
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
        private static readonly object Sync = new object();

        private PerfMachine()
        {
        }

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
            lock (Sync)
            {
                var machine = new PerfMachine();

                var query = new ManagementObjectSearcher("SELECT * From Win32_ComputerSystem");
                foreach (ManagementObject obj in query.Get())
                {
                    machine.Ram = long.Parse(obj["TotalPhysicalMemory"].ToString());
                    break;
                }

                query = new ManagementObjectSearcher("SELECT * From Win32_Processor");
                foreach (ManagementObject obj in query.Get())
                {
                    machine.Cpu = (string)obj["Name"];
                    machine.CpuHz = long.Parse(obj["CurrentClockSpeed"].ToString());
                    break;
                }

                return machine;
            }
        }

        [XmlAttribute("ram")]
        public long Ram { get; set; }

        [XmlIgnore]
        public double RamMb
        {
            get
            {
                return this.Ram / (1024d * 1024);
            }
        }

        [XmlAttribute("cpu")]
        public string Cpu { get; set; }

        [XmlAttribute("cpu-freq")]
        public long CpuHz { get; set; }

        [XmlIgnore]
        public double CpuMHz
        {
            get
            {
                return this.CpuHz / 1.0e6;
            }
        }
    }
}
