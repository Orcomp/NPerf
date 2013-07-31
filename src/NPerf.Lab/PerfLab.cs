namespace NPerf.Lab
{
    using System;
    using System.Collections.Generic;

    using System.IO;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Reflection;
    using Fasterflect;
    using NPerf.Core.Info;
    using NPerf.Core.PerfTestResults;
    using NPerf.Framework;

    public class PerfLab
    {
        private readonly List<TestSuiteInfo> testSuites = new List<TestSuiteInfo>();

        private IDictionary<Guid, TestInfo> tests;

        private List<Assembly> loadedAssemblies = new List<Assembly>();

        public PerfLab(Assembly fixtureLib, params Assembly[] testSubjects)
        {
            this.SystemInfo = SystemInfo.Instance;

            this.testSuites = (from tester in fixtureLib.TypesWith<PerfTesterAttribute>()
                               select TestSuiteManager.GetTestSuiteInfo(tester, testSubjects.Distinct()
                                                                                            .SelectMany(t => t.Types())
                                                                                            .Where(
                                                                                                testedType =>
                                                                                                IsTestableType(tester,
                                                                                                               testedType))))
                .ToList();
            this.tests = this.testSuites.SelectMany(suite => suite.Tests)
                             .ToDictionary(test => test.TestId, test => test);

            loadedAssemblies.AddRange(testSubjects.Union(new[] {fixtureLib})
                                                  .Distinct());
        }

        public PerfLab(params Assembly[] perfTestAssemblies)
        {
            this.SystemInfo = SystemInfo.Instance;            
            
            this.testSuites = (from assembly in perfTestAssemblies.Distinct()
                               from tester in assembly.TypesWith<PerfTesterAttribute>()
                               select
                                   TestSuiteManager.GetTestSuiteInfo(tester,
                                                                     perfTestAssemblies.SelectMany(t => t.Types())
                                                                                       .Where(
                                                                                           testedType =>
                                                                                           IsTestableType(tester,
                                                                                                          testedType))))
                .ToList();

            this.tests = this.testSuites.SelectMany(suite => suite.Tests)
                             .ToDictionary(test => test.TestId, test => test);

            loadedAssemblies.AddRange(perfTestAssemblies.Distinct());
        }

        public void AddAssemblies(params Assembly[] assemblies)
        {
            var newAssemblies = (from assembly in assemblies
                                 where !loadedAssemblies.Contains(assembly)
                                 select assembly).ToArray();

            var allAssemblies = loadedAssemblies.Union(newAssemblies).Distinct().ToArray();

            var newTestSuites = (from assembly in allAssemblies
                                 from tester in assembly.TypesWith<PerfTesterAttribute>()
                                 select
                                     TestSuiteManager.GetTestSuiteInfo(tester, allAssemblies
                                                                                   .SelectMany(t => t.Types())
                                                                                   .Where(
                                                                                       testedType =>
                                                                                       IsTestableType(tester,
                                                                                                      testedType)),
                                                                       this.testSuites.ToArray()));


            this.testSuites.AddRange(newTestSuites.Where(x => !testSuites.Contains(x)).ToArray());

            loadedAssemblies.AddRange(newAssemblies);
            this.tests = this.testSuites.SelectMany(suite => suite.Tests)
                             .ToDictionary(test => test.TestId, test => test);
        }

        private static bool IsTestableType(Type testerType, Type testedType)
        {
            var testerAttr = testerType.Attribute<PerfTesterAttribute>();
            return testedType.IsPublic && !testedType.IsGenericTypeDefinition
                   && testerAttr.TestedType.IsAssignableFrom(testedType) && !(testerAttr.TestedType == testedType)
                   && !testedType.IsAbstract && !testedType.IsInterface;
        }

        public SystemInfo SystemInfo { get; private set; }

        public IDictionary<Guid, TestInfo> Tests
        {
            get
            {
                return this.tests;
            }
        }

        public IEnumerable<TestSuiteInfo> TestSuites 
        {
            get
            {
                return this.testSuites.AsEnumerable();
            }
        }

        public IObservable<PerfTestResult> Run(Guid[] tests, bool parallel = false)
        {
            return TestSuiteManager.Run(tests.Select(x => this.Tests[x]).ToArray(), parallel);
        }

        public IObservable<PerfTestResult> Run(Guid[] tests, int start, int step, int end, bool parallel = false)
        {
            return TestSuiteManager.Run(tests.Select(x => this.Tests[x]).ToArray(), start, step, end, parallel);
        }

        public IObservable<PerfTestResult> Run(bool parallel = false)
        {
            return this.testSuites.ToObservable().SelectMany(suite => TestSuiteManager.Run(suite, parallel));
        }

        public IObservable<PerfTestResult> Run(int start, int step, int end, bool parallel = false)
        {
            return this.testSuites.ToObservable().SelectMany(suite => TestSuiteManager.Run(suite, start, step, end, parallel));
        }
    }
}
