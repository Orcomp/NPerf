namespace NPerf.Lab
{
    using System;
    using System.Reactive.Disposables;
    using System.Reactive.Subjects;
    using System.Threading;
    using System.Threading.Tasks;
    using NPerf.Core.Communication;
    using NPerf.Core.PerfTestResults;

    internal class SingleExperimentListener : IObservable<PerfTestResult>, IDisposable
    {
        private readonly ExperimentProcess experiment;

        private readonly ProcessMailBox mailBox;

        private readonly Action<ExperimentProcess> startProcess;

        public SingleExperimentListener(ExperimentProcess experiment, Action<ExperimentProcess> startProcess)
        {
            this.startProcess = startProcess;                 
            this.experiment = experiment;
            this.mailBox = new ProcessMailBox(this.experiment.ChannelName, TimeSpan.FromMilliseconds(-1));
        }

        protected void Run(IObserver<PerfTestResult> observer)
        {
            var ok = true;
            try
            {
                this.startProcess(this.experiment);

                object message;
                Thread.CurrentThread.Name = this.mailBox.ChannelName;

                var buff = new ReplaySubject<PerfTestResult>();
                buff.Subscribe(observer);
                do
                {
                    message = this.mailBox.Content as PerfTestResult;
                    
                    if (message != null)
                    {
                        buff.OnNext((PerfTestResult)message);
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
            var task = Task.Factory.StartNew(() => this.Run(observer));           

            return Disposable.Create(
                () =>
                    {
                        task.Wait(TimeSpan.FromMilliseconds(10));
                    });
        }

        public void Dispose()
        {
            this.mailBox.Dispose();
        }
    }
}
