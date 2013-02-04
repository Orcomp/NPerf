namespace NPerf.Lab
{
    using System.Linq;
    using NPerf.Core.PerfTestResults;
    using NPerf.Lab.Threading;
    using System.Reactive.Subjects;

    internal class MultiExperimentListener : MultiRunnable
    {
        public MultiExperimentListener(string[] names, ISubject<TestResult> subject)
        {
            this.SetRunnables((from name in names
                               select (IRunnable)new SingleExperimentListener(name, subject)).ToArray());
        }
    }
}
