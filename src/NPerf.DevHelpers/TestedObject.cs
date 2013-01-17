namespace NPerf.DevHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class TestedObject : ITestedObject
    {
        public void SomeAction()
        {
            Console.Out.WriteLine("The action in tested object is executed.");
        }
    }
}
