namespace NPerf.Lab
{
    using System;
    using System.Management;

    public class SystemInfo
    {
        private static readonly SystemInfo instance = new SystemInfo();

        private SystemInfo()
        {
            this.OsName = Environment.OSVersion.Platform.ToString();
            this.OsVersion = Environment.Version.ToString();

            using (var query = new ManagementObjectSearcher("SELECT * From Win32_ComputerSystem"))
            {
                foreach (ManagementObject obj in query.Get())
                {
                    this.Ram = long.Parse(obj["TotalPhysicalMemory"].ToString());
                    break;
                }
            }

            using (var query = new ManagementObjectSearcher("SELECT * From Win32_Processor"))
            {
                foreach (ManagementObject obj in query.Get())
                {
                    this.Cpu = (string)obj["Name"];
                    this.CpuHz = long.Parse(obj["CurrentClockSpeed"].ToString());
                }
            }
        }
        
        public static SystemInfo Instance
        {
            get
            {
                return instance;
            }
        }

        public string OsName { get; private set; }

        public string OsVersion { get; private set; }

        public long Ram { get; private set; }

        public double RamMb
        {
            get
            {
                return this.Ram / (1024d * 1024);
            }
        }

        public string Cpu { get; private set; }

        public long CpuHz { get; private set; }

        public double CpuMHz
        {
            get
            {
                return this.CpuHz / 1.0e6;
            }
        }
    }
}
