﻿namespace NPerf.Experiment
{
    using System;
    using System.Linq;
    using NPerf.Core;
    using NPerf.Core.PerfTestResults;

    internal class ExperimentScope
    {
        public static void Start(StartParameters startParameters)
        {            
            using (var testObserver = new TestObserver(startParameters.ChannelName))
            {
                var suite = AssemblyLoader.CreateInstance<PerfTestSuite>(
                    startParameters.SuiteAssembly, startParameters.SuiteType);

                var subject = AssemblyLoader.CreateInstanceFullyQualifiedName(
                    startParameters.SubjectAssembly, startParameters.SubjectType);

                var start = startParameters.Start;
                var step = startParameters.Step;
                var end = startParameters.End;
                
                if (suite != null && subject != null)
                {
                    var test = suite.Tests.First(x => x.TestMethodName == startParameters.TestMethod && x.TestedType == subject.GetType());

                    PerfTestResultFactory.Instance.Init(test.TestId);

                    var runner = new TestRunner(
                        delegate(int idx) { suite.SetUp(idx, subject); },
                        delegate { test.Test(subject); },
                        delegate { suite.TearDown(subject); },
                        delegate(int idx) { return suite.GetRunDescriptor(idx); },
                        string.IsNullOrEmpty(start) ? 0 : int.Parse(start),
                        string.IsNullOrEmpty(step) ? 1 : int.Parse(step),
                        string.IsNullOrEmpty(end) ? suite.DefaultTestCount : int.Parse(end),
                        startParameters.IgnoreFirstRunDueToJITting,
                        startParameters.TriggerGCBeforeEachTest);

                    runner.Subscribe(testObserver);
                    testObserver.OnCompleted();
                }
                else
                {
                    testObserver.OnError(new Exception());
                }
            }
        }
    }
}
