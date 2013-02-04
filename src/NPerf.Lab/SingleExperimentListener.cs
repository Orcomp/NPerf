namespace NPerf.Lab
{
    using NPerf.Core;
    using NPerf.Core.Communication;
    using NPerf.Core.PerfTestResults;
    using NPerf.Lab.Threading;
    using System.Threading;
    using System.Reactive.Subjects;

    internal class SingleExperimentListener : SingleRunnable
    {
        private ProcessMailBox mailBox;

        private ISubject<PerfTestResult> subject;

        public SingleExperimentListener(string name, ISubject<PerfTestResult> subject)
            : base(true, false)
        {
            this.mailBox = new ProcessMailBox(name);
            this.subject = subject;
        }

        protected override void Run()
        {
            object message;
            Thread.CurrentThread.Name = this.mailBox.ChannelName;
            do
            {
                message = this.mailBox.Content as PerfTestResult;
                if (message != null)
                {
                    this.subject.OnNext((PerfTestResult)message);
                }
            }
            while (!(message is ExperimentError && ((PerfTestResult)message).Descriptor == -1) && !(message is ExperimentCompleted));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.mailBox.Dispose();
            }

            this.mailBox = null;
            this.subject = null;

            base.Dispose(disposing);
        }
    }
}
