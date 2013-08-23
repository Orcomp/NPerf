namespace NPerf.Lab
{
    using System;
    using System.Diagnostics;
    using System.Text;

    public class ExperimentProcess : IDisposable
    {
        private Process experimentProcess;

        private readonly string suiteAssemblyLocation;

        private readonly string suiteTypeName;
        
        private readonly Type testedType;

        private readonly Type testerType;

        private readonly string testName;

        public ExperimentProcess(string channelName, string suiteAssemblyLocation, string suiteTypeName, Type testerType, Type testedType, string testName)
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
            this.suiteAssemblyLocation = suiteAssemblyLocation;
            this.suiteTypeName = suiteTypeName;
            this.testerType = testerType;
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
                    "-suiteLib {0} -suiteType {1} -testMethod {2} -testerAssm {3} -subjectAssm {4} -subjecType {5} -channelName {6} -start {7} -step {8} -end {9}",
                    this.suiteAssemblyLocation,
                    this.suiteTypeName,
                    this.testName,
                    testerType.Assembly.Location,
                    this.testedType.Assembly.Location,
                    this.testedType.FullName, // Is necesary to load generics to use FullName instead of Name
                    this.ChannelName,
                    start,
                    step,
                    end);

            this.Run(waitForExit);
            started = true;
        }



        public void Start(bool waitForExit = true)
        {
            this.experimentProcess.StartInfo.Arguments =
                string.Format(
                    "-suiteLib {0} -suiteType {1} -testMethod {2} -testerAssm {3} -subjectAssm {4} -subjecType {5} -channelName {6}",
                    this.suiteAssemblyLocation,
                    this.suiteTypeName,
                    this.testName,
                    testerType.Assembly.Location,
                    this.testedType.Assembly.Location,
                    this.testedType.FullName, // Is necesary to load generics to use FullName instead of Name
                    this.ChannelName);
            
            this.Run(waitForExit);
            started = true;
        }

        private bool started = false;

        public void Stop()
        {
            if (!started)
            {
                return;
            }

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
