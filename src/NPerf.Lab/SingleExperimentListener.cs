namespace NPerf.Lab
{
    using System;
    using System.Reactive.Disposables;
    using System.Threading;
    using NPerf.Core.Communication;
    using NPerf.Core.PerfTestResults;

    internal class SingleExperimentListener : IObservable<PerfTestResult>, IDisposable
    {
        private readonly ExperimentProcess experiment;

        private readonly ProcessMailBox mailBox;

        private static EventWaitHandle wh = new AutoResetEvent(true);

        private bool parallel;

        private readonly Action<ExperimentProcess> startProcess;

        public SingleExperimentListener(ExperimentProcess experiment, Action<ExperimentProcess> startProcess, bool parallel = false)
        {
            this.startProcess = startProcess;
            this.parallel = parallel;                   
            this.experiment = experiment;
            this.mailBox = new ProcessMailBox(this.experiment.ChannelName);
        }

        protected void Run(IObserver<PerfTestResult> observer)
        {
            var ok = true;
            try
            {
                this.startProcess(this.experiment);

                object message;
                Thread.CurrentThread.Name = this.mailBox.ChannelName;
                do
                {
                    message = this.mailBox.Content as PerfTestResult;
                    if (message != null)
                    {
                        observer.OnNext((PerfTestResult)message);
                    }
                }
                while (!(message is ExperimentError && ((PerfTestResult)message).Descriptor == -1)
                       && !(message is ExperimentCompleted) && !this.experiment.HasExited);            
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                ok = false;
                observer.OnError(ex);
            }

            if (ok)
            {
                observer.OnCompleted();
            }
        }

        public IDisposable Subscribe(IObserver<PerfTestResult> observer)
        {
            if (!this.parallel)
            {
                wh.WaitOne();
            }

            var thread = new Thread(obj => this.Run((IObserver<PerfTestResult>)obj));
            thread.Start(observer);
            

            return Disposable.Create(
                () =>
                    {
                        if (!this.parallel)
                        {
                            wh.Set();
                        }
                    });
        }

        public void Dispose()
        {
            this.mailBox.Dispose();
        }
    }
}
