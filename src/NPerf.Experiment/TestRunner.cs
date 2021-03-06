﻿namespace NPerf.Experiment
{
    using System;
    using NPerf.Core.Communication.Monitoring;
    using NPerf.Core.PerfTestResults;

    internal class TestRunner : IObservable<PerfTestResult>
    {
        private readonly Action testMethod;

        private readonly Action<int> setUpMethod;

        private readonly Action tearDownMethod;

        private readonly Func<int, double> descriptorMethod;

        private readonly int start;

        private readonly int step;

        private readonly int end;

        private bool ignoreFirstRunDueToJITting { get; set; }

        private bool triggerGCBeforeEachTest { get; set; }

        public TestRunner(
            Action<int> setUpMethod,
            Action testMethod,
            Action tearDownMethod,
            Func<int, double> descriptorMethod,
            int start,
            int step,
            int end,
            bool ignoreFirstRunDueToJITting = false,
            bool triggerGCBeforeEachTest = false)
        {
            this.testMethod = testMethod;
            this.setUpMethod = setUpMethod;
            this.tearDownMethod = tearDownMethod;
            this.descriptorMethod = descriptorMethod;
            this.start = start;
            this.step = step;
            this.end = end;
            this.ignoreFirstRunDueToJITting = ignoreFirstRunDueToJITting;
            this.triggerGCBeforeEachTest = triggerGCBeforeEachTest;
        }

        public IDisposable Subscribe(IObserver<PerfTestResult> observer)
        {
            if (ignoreFirstRunDueToJITting)
            {
                this.setUpMethod(this.start);
                try
                {
                    this.testMethod();
                }
                catch (Exception ex)
                {

                }
                this.tearDownMethod();
            }

            var time = new DurationMonitor();
            var memory = new MemoryMonitor();

            for (var i = this.start; i < this.end; i += this.step)
            {
                var ok = true;

                var descriptor = this.descriptorMethod(i);
                this.setUpMethod(i);

                // clean memory
                if (this.triggerGCBeforeEachTest)
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();   
                }

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
                        observer.OnNext(PerfTestResultFactory.Instance.FaultResult(descriptor, ex));
                    }
                }

                this.tearDownMethod();
                if (ok)
                {
                    observer.OnNext(PerfTestResultFactory.Instance.PerfResult(time.Value, memory.Value, descriptor));
                }
            }

            return new DisposableScope();
        }
    }
}
