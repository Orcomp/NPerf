namespace NPerf.ConsoleTester
{
    using System;
    using System.Collections.Generic;
    using System.Perf.StringBuilding;
    using System.Windows.Forms;
    using NPerf.Core.PerfTestResults;
    using NPerf.Lab;

    internal class ExecutionContext : ApplicationContext
    {
        private List<TestResult> list = new List<TestResult>();

        public ExecutionContext()
        {
            this.RunTests();
        }

        public void RunTests()
        {
            var lab = new PerfLab(typeof(StringBuildingTester).Assembly, typeof(StringRunner).Assembly, typeof(Dictionary<,>).Assembly);

            lab.Run(true).Subscribe(this.OnNext, ex => { }, this.Completed);
            
            //Application.Exit();
        }

        void OnNext(TestResult value)
        {
            this.list.Add(value);
            Console.WriteLine(value);
        }

        private void Completed()
        {
            this.ExitThread();
           // Application.ExitThread();
         //   Application.Exit();
        }
    }
}
