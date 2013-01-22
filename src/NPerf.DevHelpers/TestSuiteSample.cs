namespace NPerf.DevHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;


    using NPerf.Framework;
    using NPerf.Framework.Interfaces;

    public class TestSuiteSample : IPerfTestSuite
    {
        public IPerfTest[] Tests
        {
            get { throw new NotImplementedException(); }
        }

        public int DefaultTestCount
        {
            get { throw new NotImplementedException(); }
        }

        public string Description
        {
            get { throw new NotImplementedException(); }
        }

        public string FeatureDescription
        {
            get { throw new NotImplementedException(); }
        }

        public int CurrentIteration
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void SetUp(int iteration, object testedObject)
        {
            throw new NotImplementedException();
        }

        public void TearDown(object testedObject)
        {
            throw new NotImplementedException();
        }


        public double GetRunDescriptor(int iteration)
        {
            throw new NotImplementedException();
        }
    }
}
