namespace NPerf.Lab
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Reflection;
    using Fasterflect;
    using NPerf.Core.PerfTestResults;
    using NPerf.Framework;
    using NPerf.Framework.Interfaces;
    using NPerf.Lab.Info;
    using NPerf.Lab.Queueing;
    using NPerf.Lab.TestBuilder;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    internal class TestSuiteManager
    {
        public static TestSuiteInfo GetTestSuiteInfo(Type testerType, Type testedType)
        {
            CheckArguments(testerType, testedType);
            var testerAttribute = testerType.Attribute<PerfTesterAttribute>();
            
            var suiteInfo = new TestSuiteInfo
                           {
                               TesterType = testerType,
                               TestedType = testedType,
                               DefaultTestCount = testerAttribute.TestCount,
                               TestSuiteDescription = testerAttribute.Description,
                               FeatureDescription = testerAttribute.FeatureDescription,
                               TestedAbstraction = testerAttribute.TestedType
                           };

            var tests = new List<TestInfo>();
            var ignoredTests = new List<TestInfoIgnored>();

            foreach (var test in from method in testerType.MethodsWith(Flags.AllMembers, typeof(PerfTestAttribute))
                                 select GetTestInfo(Guid.NewGuid(), method, testerAttribute.TestedType, suiteInfo))
            {
                if (test is TestInfo)
                {
                    tests.Add((TestInfo)test);
                    continue;
                }

                if (test is TestInfoIgnored)
                {
                    ignoredTests.Add((TestInfoIgnored)test);
                    continue;
                }

                throw new NotImplementedException("Unknown test information type.");
            }

            suiteInfo.Tests = tests.ToArray();
            suiteInfo.IgnoredTests = ignoredTests.ToArray();

            return suiteInfo;
        }

        private static string BuildTestSuiteAssembly(TestSuiteInfo testSuiteInfo)
        {
            var builder = new TestSuiteBuilder(testSuiteInfo);
            return builder.Build();
        }

        private static IPerfTestInfo GetTestInfo(Guid id, MethodInfo method, Type testedAbstraction, TestSuiteInfo suiteInfo )
        {
            var testAttribute = method.Attribute<PerfTestAttribute>();
            if (testAttribute == null)
            {
                throw new ArgumentNullException("method");
            }

            if (method.ReturnType != typeof(void) || !method.HasParameterSignature(new[] { testedAbstraction }))
            {
                throw new ArgumentException("Incorrect parameter signature");
            }

            IPerfTestInfo result;
            var ignoreAttribute = method.Attribute<PerfIgnoreAttribute>();
            if (ignoreAttribute == null)
            {
                result = new TestInfo
                            {
                                TestId = id,
                                TestDescription = testAttribute.Description,
                                TestMethodName = method.Name, 
                                Suite = suiteInfo
                            };
            }
            else
            {
                result = new TestInfoIgnored
                {
                    TestId = id,
                    TestDescription = testAttribute.Description,
                    TestMethodName = method.Name,
                    IgnoreMessage = ignoreAttribute.Message,
                    Suite = suiteInfo
                };
            }

            return result;
        }

        private static void CheckArguments(Type testerType, Type testedType)
        {
            if (testerType == null)
            {
                throw new ArgumentNullException("testerType");
            }

            if (testedType == null)
            {
                throw new ArgumentNullException("testedType");
            }

            var testerAttribute = testerType.Attribute<PerfTesterAttribute>();
            if (testerAttribute == null)
            {
                throw new ArgumentException("Tester type must be marked by PerfTesterAttribute", "testerType");
            }

            if (!testerAttribute.TestedType.IsAssignableFrom(testedType))
            {
                throw new ArgumentException(
                    string.Format("Tester type {0} is not assignable from {1}", testerAttribute.TestedType, testedType),
                    "testerType");
            }
        }

        private static IObservable<TestResult> CreateRunObservable(TestSuiteInfo testSuiteInfo, Action<MultiExperimentProcess> startProcess)
        {
            return Observable.Create<TestResult>(
                observer =>
                    {
                        var assemblyLocation = BuildTestSuiteAssembly(testSuiteInfo);
                        var queue = new ReactiveQueue<TestResult>();

                        var processes = new MultiExperimentProcess(
                            (from testMethod in testSuiteInfo.Tests
                             select
                                 new ExperimentProcess(
                                 string.Format(
                                     "{0}.{1}({2})",
                                     testSuiteInfo.TesterType.Name,
                                     testMethod.TestMethodName,
                                     testSuiteInfo.TestedType.Name),
                                 assemblyLocation,
                                 TestSuiteCodeBuilder.TestSuiteClassName,
                                 testSuiteInfo.TestedType,
                                 testMethod.TestMethodName)).ToArray());

                        if (processes.Experiments.Any())
                        {
                            processes.Exited += (sender, e) => observer.OnCompleted();

                            queue.ItemDequeued += delegate(object sender, DequeueEventArgs<TestResult> e)
                                {
                                    if (e.Item != null)
                                    {
                                        observer.OnNext(e.Item);
                                    }
                                };

                            var listener =
                                new MultiExperimentListener(
                                    processes.Experiments.Select(x => x.ChannelName).ToArray(), queue);
                            listener.Start();

                            startProcess(processes);
                        }
                        else
                        {
                            observer.OnCompleted();
                        }

                        return Disposable.Create(
                            () =>
                                {
                                    processes.Dispose();
                                    queue.Dispose();
                                    if (!string.IsNullOrEmpty(assemblyLocation))
                                    {
                                        File.Delete(assemblyLocation);
                                    }
                                });
                    });
        }

        public static IObservable<TestResult> Run(TestSuiteInfo testSuiteInfo, bool parallel = false)
        {
            return CreateRunObservable(testSuiteInfo, processes => processes.Start(!parallel));
        }


        public static IObservable<TestResult> Run(TestSuiteInfo testSuiteInfo, int start, int step, int end, bool parallel = false)
        {
            return CreateRunObservable(testSuiteInfo, processes => processes.Start(start, step, end, !parallel));
        }
    }
}
