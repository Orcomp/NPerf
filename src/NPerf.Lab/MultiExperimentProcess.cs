namespace NPerf.Lab
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    internal class MultiExperimentProcess : IDisposable
    {
        private int counter;

        private static readonly object sync = new object();

        public event EventHandler Exited;

        private ExperimentProcess[] processes;

        public MultiExperimentProcess(params ExperimentProcess[] expriments)
        {
            this.processes = expriments;
            this.counter = expriments.Length;

            foreach (var expriment in expriments)
            {
                expriment.Exited += (sender, e) =>
                    {
                        lock (sync)
                        {
                            counter--;

                            if (this.counter == 0 && Exited != null)
                            {
                                Exited(this, EventArgs.Empty);
                            }

                            if (counter < 0)
                            {
                                throw new InvalidOperationException("The count of exit processes are greater than count of started processes.");
                            }
                        }
                    };
            }
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
            foreach (var process in processes)
            {
                process.Dispose();
            }
        }
    }
}
