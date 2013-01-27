﻿namespace NPerf.Experiment
{
    using System;
    using NPerf.Core;
    using NPerf.Core.Monitoring;
    using NPerf.Core.TestResults;

    internal class TestRunner : IObservable<TestResult>
    {
        private readonly Action testMethod;

        private readonly Action<int> setUpMethod;

        private readonly Action tearDownMethod;

        private readonly Func<int, double> descriptorMethod;

        private readonly int start;

        private readonly int step;

        private readonly int end;

        public TestRunner(
            Action<int> setUpMethod,
            Action testMethod,
            Action tearDownMethod,
            Func<int, double> descriptorMethod,
            int start,
            int step,
            int end)
        {
            this.testMethod = testMethod;
            this.setUpMethod = setUpMethod;
            this.tearDownMethod = tearDownMethod;
            this.descriptorMethod = descriptorMethod;
            this.start = start;
            this.step = step;
            this.end = end;
        }

        public IDisposable Subscribe(IObserver<TestResult> observer)
        {
            var time = new DurationMonitor();
            var memory = new MemoryMonitor();

            for (var i = this.start; i < this.end; i += this.step)
            {
                var ok = true;
                RunDescriptor.Instance.Value = this.descriptorMethod(i);
                this.setUpMethod(i);

                // clean memory
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

                using (time.Observe())
                using (memory.Observe())
                {
                    try
                    {
                        this.testMethod();
                    }
                    catch (Exception ex)
                    {
                        ok = false;
                        observer.OnNext(new PerfFailedResult(ex));
                    }
                }

                this.tearDownMethod();
                if (ok)
                {
                    observer.OnNext(new PerfResult(time.Value, memory.Value));
                }
            }

            return new DisposableScope();
        }
    }
}
