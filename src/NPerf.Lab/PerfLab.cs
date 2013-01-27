namespace NPerf.Lab
{
    using System;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Reflection;
    using Fasterflect;
    using NPerf.Core;
    using NPerf.Core.TestResults;
    using NPerf.Framework;

    public class PerfLab
    {
        private readonly IObservable<TestSuiteManager> testManagers;

        public PerfLab(Assembly fixtureLib, params Assembly[] testSubjects)
        {
            this.testManagers = (from tester in fixtureLib.TypesWith<PerfTesterAttribute>()
                                 let testerAttr = tester.Attribute<PerfTesterAttribute>()
                                 from testedType in testSubjects.SelectMany(t => t.Types())
                                 where testerAttr.TestedType.IsAssignableFrom(testedType)
                                 select new TestSuiteManager(tester, testedType)).ToObservable();
        }

        public IObservable<TestResult> Run()
        {
            return this.testManagers.SelectMany(testManager => testManager.Run());
        }


        public IObservable<TestResult> Run(int start, int step, int end)
        {
            return this.testManagers.SelectMany(testManager => testManager.Run(start, step, end));
        }
    }
}
