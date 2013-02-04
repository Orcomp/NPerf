namespace NPerf.Lab
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Reflection;
    using Fasterflect;
    using NPerf.Core.PerfTestResults;
    using NPerf.Framework;
    using NPerf.Lab.Info;

    public class PerfLab
    {
        private readonly TestSuiteInfo[] testSuites;

        private readonly IDictionary<Guid, TestInfo> tests;

        public PerfLab(Assembly fixtureLib, params Assembly[] testSubjects)
        {
            this.SystemInfo = SystemInfo.Instance;
            
            this.testSuites = (from tester in fixtureLib.TypesWith<PerfTesterAttribute>()
                               from testedType in testSubjects.SelectMany(t => t.Types())
                               where this.IsTestebleType(tester, testedType)                                   
                               select TestSuiteManager.GetTestSuiteInfo(tester, testedType)).ToArray();
            this.tests = this.testSuites.SelectMany(suite => suite.Tests).ToDictionary(test => test.TestId, test => test);
        }

        private bool IsTestebleType(Type testerType, Type testedType)
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

        public IObservable<TestResult> Run(Guid[] tests, bool parallel = false)
        {
            return this.testSuites.ToObservable().SelectMany(suite => TestSuiteManager.Run(tests.Select(x => this.Tests[x]).ToArray() , parallel));
        }

        public IObservable<TestResult> Run(Guid[] tests, int start, int step, int end, bool parallel = false)
        {
            return this.testSuites.ToObservable().SelectMany(suite => TestSuiteManager.Run(tests.Select(x => this.Tests[x]).ToArray(), start, step, end, parallel));
        }

        public IObservable<TestResult> Run(bool parallel = false)
        {
            return this.testSuites.ToObservable().SelectMany(suite => TestSuiteManager.Run(suite, parallel));
        }

        public IObservable<TestResult> Run(int start, int step, int end, bool parallel = false)
        {
            return this.testSuites.ToObservable().SelectMany(suite => TestSuiteManager.Run(suite, start, step, end, parallel));
        }
    }
}
