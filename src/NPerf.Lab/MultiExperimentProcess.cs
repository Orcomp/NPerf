namespace NPerf.Lab
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    internal class MultiExperimentProcess : IDisposable
    {
        public event EventHandler Exited;

        private ExperimentProcess[] processes;

        public MultiExperimentProcess(params ExperimentProcess[] expriments)
        {
            this.processes = expriments;
        }

        public IEnumerable<ExperimentProcess> Experiments
        {
            get
            {
                return this.processes.AsEnumerable();
            }
        }

        public void Start(bool waitForExit = false)
        {
            foreach (var expriment in this.processes)
            {
                expriment.Start(waitForExit);
            }
        }

        public void Start(int start, int step, int end, bool waitForExit = true)
        {
            foreach (var expriment in this.processes)
            {
                expriment.Start(start, step, end, waitForExit);
            }
        }

        public StringBuilder ReceivedErrors
        {
            get
            {
                var str = new StringBuilder();
                foreach (var process in this.processes)
                {
                    str.AppendLine(process.ReceivedErrors.ToString());
                }

                return str;
            }
        }

        public void Dispose()
        {
            foreach (var process in this.processes)
            {
                process.Dispose();
            }
        }
    }
}
