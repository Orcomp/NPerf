namespace NPerf.ConsoleTester
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Perf.StringBuilding;
    using System.Windows.Forms;
    using NPerf.Core.PerfTestResults;
    using NPerf.Lab;

    internal class ExecutionContext : ApplicationContext
    {
        private List<TestResult> list = new List<TestResult>();

        private PerfLab lab;
        public ExecutionContext()
        {
            this.lab = new PerfLab(typeof(StringBuildingTester).Assembly, typeof(StringRunner).Assembly, typeof(Dictionary<,>).Assembly);
            this.RunTests();
        }

        public void RunTests()
        {
            this.lab.Run(true).Subscribe(this.OnNext, ex => { }, this.Completed);
        }

        void OnNext(TestResult value)
        {
            this.list.Add(value);
            Console.WriteLine(value);
        }

        private void Completed()
        {
            Console.WriteLine(
                "received results count: {0}",
                this.list.Count(x => (x is NextResult) || ((x is ExperimentError) && x.Descriptor != -1)));
            Console.WriteLine(
                "awaited result count: {0}", this.lab.Tests.Select(x => x.Value.Suite.DefaultTestCount).Sum());
            Console.ReadKey();
            this.ExitThread();
        }
    }
}
