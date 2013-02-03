namespace NPerf.Lab
{
    using NPerf.Core;
    using NPerf.Core.Communication;
    using NPerf.Core.PerfTestResults;
    using NPerf.Lab.Queueing;
    using NPerf.Lab.Threading;
    using System.Threading;

    internal class SingleExperimentListener : SingleRunnable
    {
        private ProcessMailBox mailBox;

        private ReactiveQueue<TestResult> queue;

        public SingleExperimentListener(string name, ReactiveQueue<TestResult> queue)
            : base(true, false)
        {
            this.mailBox = new ProcessMailBox(name);
            this.queue = queue;
        }

        protected override void Run()
        {
            object message;
            Thread.CurrentThread.Name = this.mailBox.ChannelName;
            do
            {
                message = this.mailBox.Content as TestResult;
                if (message != null)
                {
                    this.queue.Enqueue((TestResult)message);
                }
            }
            while (!(message is ExperimentError && ((TestResult)message).Descriptor == -1) && !(message is ExperimentCompleted));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.mailBox.Dispose();
            }

            this.mailBox = null;
            this.queue = null;

            base.Dispose(disposing);
        }
    }
}
