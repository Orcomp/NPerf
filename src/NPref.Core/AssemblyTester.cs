namespace NPref.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using NPerf.Framework;

    public class AssemblyTester<T>
    {
        public AssemblyTester(IFixture<T> fixture, IPerfMonitorsManager monitors)
        {
            T[] testedObjects = InitializeTestedObjects();
        }
        
        private T[] InitializeTestedObjects()
        {
            // TODO: this part of code should be dinamically generated in future implementation
            throw new NotImplementedException();
        }
    }
}
