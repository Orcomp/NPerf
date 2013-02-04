namespace NPerf.Lab
{
    using System;
    using System.Diagnostics;
    using System.Text;

    public class ExperimentProcess : IDisposable
    {
        private Process experimentProcess;

        private readonly string suiteAssembly;

        private readonly string suiteTypeName;
        
        private readonly Type testedType;

        private readonly string testName;

        public ExperimentProcess(string channelName, string suiteAssembly, string suiteTypeName, Type testedType, string testName)
        {
            this.experimentProcess = new Process
                                         {
                                             StartInfo =
                                                 new ProcessStartInfo
                                                     {
                                                         UseShellExecute = false,
                                                         RedirectStandardError = true,
                                                         RedirectStandardOutput = false,
                                                         CreateNoWindow = true,
                                                         WindowStyle = ProcessWindowStyle.Hidden,
                                                         FileName =
                                                             Environment.CurrentDirectory
                                                             + "\\NPerf.Experiment.exe"
                                                     },
                                         };

            this.ChannelName = channelName;
            this.suiteAssembly = suiteAssembly;
            this.suiteTypeName = suiteTypeName;
            this.testedType = testedType;
            this.testName = testName;

            this.experimentProcess.ErrorDataReceived += this.experimentProcess_ErrorDataReceived;
        }

        void experimentProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (this.ReceivedErrors == null)
            {
                this.ReceivedErrors = new StringBuilder();
            }

            this.ReceivedErrors.AppendFormat(e.Data).AppendLine();
        }

        public string ChannelName { get; private set; }

        public StringBuilder ReceivedErrors { get; private set; }

        public void Start(int start, int step, int end, bool waitForExit = true)
        {
            this.experimentProcess.StartInfo.Arguments =
                string.Format(
                    "-suiteLib {0} -suiteType {1} -testMethod {2} -subjectAssm {3} -subjecType {4} -channelName {5} -start {6} -step {7} -end {8}",
                    this.suiteAssembly,
                    this.suiteTypeName,
                    this.testName,
                    this.testedType.Assembly.Location,
                    this.testedType.Name,
                    this.ChannelName,
                    start,
                    step,
                    end);

            this.Run(waitForExit);
        }

        public void Start(bool waitForExit = true)
        {
            this.experimentProcess.StartInfo.Arguments =
                string.Format(
                    "-suiteLib {0} -suiteType {1} -testMethod {2} -subjectAssm {3} -subjecType {4} -channelName {5}",
                    this.suiteAssembly,
                    this.suiteTypeName,
                    this.testName,
                    this.testedType.Assembly.Location,
                    this.testedType.Name,
                    this.ChannelName);
            
            this.Run(waitForExit);
        }

        public void Stop()
        {
            if (!this.experimentProcess.HasExited)
            {
                this.experimentProcess.Kill();
            }

            this.experimentProcess.WaitForExit();
        }

        public bool HasExited
        {
            get
            {
                return this.experimentProcess.HasExited;
            }
        }

        private void Run(bool waitForExit = true)
        {
            this.experimentProcess.Start();
            /*
            if (waitForExit)
            {
                this.experimentProcess.WaitForExit();
            }*/
        }

        public void Dispose()
        {
            this.Stop();
            this.experimentProcess.Dispose();
        }
    }
}
