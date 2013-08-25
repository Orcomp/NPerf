namespace NPerf.Lab
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using System.Reflection;
    using System.Threading.Tasks;
    using Fasterflect;
    using NPerf.Core.Info;
    using NPerf.Core.PerfTestResults;
    using NPerf.Framework;
    using NPerf.Lab.TestBuilder;
    using Fasterflect;

    internal class TestSuiteManager
    {
        public static TestSuiteInfo GetTestSuiteInfo(Type testerType, IEnumerable<Type> testedTypes, params TestSuiteInfo[] testSuitesForUpdate)
        {            
            var testerAttribute = testerType.Attribute<PerfTesterAttribute>();
            var descrGetter = testerType.MethodsWith(Flags.AllMembers, typeof(PerfRunDescriptorAttribute)).FirstOrDefault();

            var suiteInfo = testSuitesForUpdate.FirstOrDefault(x => x.TesterType == testerType) ?? new TestSuiteInfo
                {
                    TesterType = testerType,
                    DefaultTestCount = testerAttribute.TestCount,
                    TestSuiteDescription = testerAttribute.Description,
                    FeatureDescription = testerAttribute.FeatureDescription,
                    TestedAbstraction = testerAttribute.TestedType,
                    GetDescriptoMethodName = descrGetter == null ? string.Empty : descrGetter.Name
                };

            var tests = new List<TestInfo>();
            var ignoredTests = new List<TestInfoIgnored>();

            foreach (var test in from method in testerType.MethodsWith(Flags.AllMembers, typeof(PerfTestAttribute))
                                 from testedType in testedTypes
                                 where suiteInfo.Tests == null || !suiteInfo.Tests.Any(x => x.TestedType == testedType)
                                 select GetTestInfo(Guid.NewGuid(), method, testerAttribute.TestedType, suiteInfo, testedType))
            {
                var ignored = test as TestInfoIgnored;
                if (ignored != null)
                {
                    ignoredTests.Add(ignored);
                    continue;
                }

                if (test != null)
                {
                    tests.Add(test);
                    continue;
                }

                throw new NotImplementedException("Unknown test information type.");
            }

            if (suiteInfo.Tests != null)
            {
                tests.AddRange(suiteInfo.Tests);
            }
            suiteInfo.Tests = tests.ToArray();

            if (suiteInfo.IgnoredTests != null)
            {
                ignoredTests.AddRange(suiteInfo.IgnoredTests);
            }
            suiteInfo.IgnoredTests = ignoredTests.ToArray();

            return suiteInfo;
        }

        private static string BuildTestSuiteAssembly(TestSuiteInfo testSuiteInfo)
        {
            var builder = new TestSuiteBuilder(testSuiteInfo);
            return builder.Build();
        }

        private static TestInfo GetTestInfo(Guid id, MethodInfo method, Type testedAbstraction, TestSuiteInfo suiteInfo, Type testedType)
        {
            CheckTestability(suiteInfo.TesterType, testedType);

            var testAttribute = method.Attribute<PerfTestAttribute>();
            if (testAttribute == null)
            {
                throw new ArgumentNullException("method");
            }

            if (method.ReturnType != typeof(void) || !method.HasParameterSignature(new[] { testedAbstraction }))
            {
                throw new ArgumentException("Incorrect parameter signature");
            }

            if (testedType.IsGenericType)
            {
                // Fill the generic type arguments of the loaded generic type
                // with the tested abstraction interface actual generic type arguments.
                // Example: tested abstraction = IList<int>, tested type = List<T>
                // This line converts List<T> in List<int>
                testedType = testedType.MakeGenericType(testedAbstraction.GetGenericArguments());
            }

            TestInfo result;
            var ignoreAttribute = method.Attribute<PerfIgnoreAttribute>();
            if (ignoreAttribute == null)
            {
                result = new TestInfo
                            {
                                TestId = id,
                                TestDescription = testAttribute.Description,
                                TestMethodName = method.Name, 
                                TestedType = testedType,
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
                    TestedType = testedType,
                    Suite = suiteInfo
                };
            }

            return result;
        }

        private static void CheckTestability(Type testerType, Type testedType)
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

            if ((!testerAttribute.TestedType.IsAssignableFrom(testedType)) &&
                (!testedType.GetInterfaces().Any(x => x.IsGenericType &&
                    x.GetGenericTypeDefinition() == testerAttribute.TestedType.GetGenericTypeDefinition())))
            {
                throw new ArgumentException(
                    string.Format("Tester type {0} is not assignable from {1}", testerAttribute.TestedType, testedType),
                    "testerType");
            }
        }

        private static IObservable<PerfTestResult> CreateRunObservable(TestSuiteInfo testSuiteInfo,
                                                                       Predicate<TestInfo> testFilter,
                                                                       Action<ExperimentProcess> startProcess,
                                                                       bool parallel = false)
        {
            return Observable.Create<PerfTestResult>(
                observer =>
                    {
                        var assemblyLocation = BuildTestSuiteAssembly(testSuiteInfo);
                        
                        var processes = new MultiExperimentProcess(
                            (from testMethod in testSuiteInfo.Tests
                             where testFilter(testMethod)
                             select
                                 new ExperimentProcess(
                                 string.Format(
                                     "{0}.{1}({2})",
                                     testSuiteInfo.TesterType.Name,
                                     testMethod.TestMethodName,
                                     testMethod.TestedType.Name),
                                 assemblyLocation,
                                 TestSuiteCodeBuilder.TestSuiteClassName,
                                 testSuiteInfo.TesterType,
                                 testMethod.TestedType,
                                 testMethod.TestMethodName)).ToArray());

                        var listeners = Observable.Empty<PerfTestResult>();

                        if (!parallel)
                        {
                            listeners = processes.Experiments.Aggregate(listeners,
                                                                   (current, experiment) =>
                                                                   current.Concat(
                                                                       new SingleExperimentListener(experiment,
                                                                                                    startProcess)));
                        }
                        else
                        {
                            listeners = from experiment in processes.Experiments.ToObservable()
                                        from result in new SingleExperimentListener(experiment, startProcess)
                                        select result;
                        }

                        IDisposable subscription = null;
                        
                        subscription = listeners.SubscribeSafe(observer);

                        return Disposable.Create(
                            () =>
                                {
                                    if (subscription != null)
                                    {
                                        subscription.Dispose();
                                        subscription = null;
                                    }

                                    processes.Dispose();

                                    if (!string.IsNullOrEmpty(assemblyLocation))
                                    {
                                        File.Delete(assemblyLocation);
                                    }
                                });
                    });
        }

        internal static IObservable<PerfTestResult> Run(TestInfo[] testInfo, bool parallel = false)
        {
            return testInfo.Select(x => x.Suite)
                           .Distinct()
                           .ToObservable()
                           .SelectMany(suite => CreateRunObservable(suite,
                                                                    x =>
                                                                    testInfo.FirstOrDefault(
                                                                        test => test.TestId.Equals(x.TestId)) != null,
                                                                    processes => processes.Start(!parallel),
                                                                    parallel));
        }

        public static IObservable<PerfTestResult> Run(TestSuiteInfo testSuiteInfo, bool parallel = false)
        {
            return CreateRunObservable(testSuiteInfo,  x => true, processes => processes.Start(!parallel), parallel);
        }

        internal static IObservable<PerfTestResult> Run(TestInfo[] testInfo, int start, int step, int end, bool parallel)
        {
            return testInfo.Select(x => x.Suite).ToObservable().Distinct()
               .SelectMany(suite => CreateRunObservable(suite,
                   x => testInfo.FirstOrDefault(test => test.TestId.Equals(x.TestId)) != null,
                   processes => processes.Start(start, step, end, !parallel), parallel));
        }

        public static IObservable<PerfTestResult> Run(TestSuiteInfo testSuiteInfo, int start, int step, int end, bool parallel = false)
        {
            return CreateRunObservable(testSuiteInfo, x => true, processes => processes.Start(start, step, end, !parallel), parallel);
        }       
    }
}
