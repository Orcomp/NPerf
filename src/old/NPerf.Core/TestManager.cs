namespace NPerf.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Reflection;
    using Fasterflect;
    using NPerf.Framework;

    public class TestManager
    {
        public TestManager(Assembly fixtureAssm, params Assembly[] assmsToTest)
        {
            // TODO: create bidimentional array with testers
            var testers = PerfTester.FromAssembly(fixtureAssm);
            foreach (var perfTester in testers)
            {
//                perfTester.LoadTestedTypes();
            }
        }
    }
}
