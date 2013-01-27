namespace NPerf.Lab
{
    using System;
    using System.Linq;
    using NPerf.Core;
    using NPerf.Core.TestResults;
    using NPerf.Lab.Queueing;
    using NPerf.Lab.TestBuilder;
    using System.Reactive.Linq;
    using System.Reactive.Disposables;
    using System.Threading.Tasks;

    internal class TestSuiteManager
    {
        private Type testerType;

        private Type testedType;

        public TestSuiteManager(Type testerType, Type testedType )
        {
            if (testerType == null)
            {
                throw new ArgumentNullException("testerType");
            }

            if (testedType == null)
            {
                throw new ArgumentNullException("testedType");
            }

            this.testerType = testerType;
            this.testedType = testedType;
        }

        private IObservable<TestResult> CreateRunObservable(Action<MultiExperimentProcess> startProcess)
        {
            return Observable.Create<TestResult>(
                observer =>
                {
                    var queue = new ReactiveQueue<TestResult>();

                    var builder = new TestSuiteBuilder(this.testerType, this.testedType);
                    var assembly = builder.Build();
                    var processes =
                        new MultiExperimentProcess(
                            (from testMethod in builder.TestMethods.Select(x => x.Name)
                             select
                                 new ExperimentProcess(
                                 string.Format(
                                     "{0}.{1}({2})", this.testerType.Name, testMethod, this.testedType.Name),
                                 assembly,
                                 TestSuiteCodeBuilder.TestSuiteClassName,
                                 this.testedType,
                                 testMethod)).ToArray());

                    processes.Exited += (sender, e) => { observer.OnCompleted(); };

                    queue.ItemDequeued += delegate(object sender, DequeueEventArgs<TestResult> e)
                    {
                        if (e.Item != null)
                        {
                            observer.OnNext(e.Item);
                        }
                    };

                    var listener = new MultiExperimentListener(processes.Experiments.Select(x => x.ChannelName).ToArray(), queue);
                    listener.Start();

                    startProcess(processes);

                    return Disposable.Empty;
                });
        }

        public IObservable<TestResult> Run(bool parallel = false)
        {
            return this.CreateRunObservable(processes => processes.Start());
        }


        public IObservable<TestResult> Run(int start, int step, int end)
        {
            return this.CreateRunObservable(processes => processes.Start(start, step, end));
        }
    }
}
