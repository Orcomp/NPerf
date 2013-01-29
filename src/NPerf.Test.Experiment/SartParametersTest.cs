using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NPerf.Experiment;
using FluentAssertions;

namespace NPerf.Test.Experiment
{
    [TestClass]
    public class SartParametersTest
    {
        [TestMethod]
        public void CanUseSetParametersWithStartStepEnd()
        {
            var args = new[] 
            {
                "-channelName channel", 
                "-suiteLib suiteAssembly", 
                "-suiteType testerType", 
                "-subjectAssm subjectAssembly",
                "-subjecType testedObjectType", 
                "-testMethod testMethodName",
                "-start 0",
                "-step 1",
                "-end 10"
            };

            var parameters = new SartParameters(args);
            parameters.ChannelName.Should().Be("channel");
            parameters.SuiteAssembly.Should().Be("suiteAssembly");
            parameters.SuiteType.Should().Be("testerType");
            parameters.SubjectAssembly.Should().Be("subjectAssembly");
            parameters.SubjectType.Should().Be("testedObjectType");
            parameters.TestMethod.Should().Be("testMethodName");
            parameters.Start.Should().Be("0");
            parameters.Step.Should().Be("1");
            parameters.End.Should().Be("10");
        }
    }
}
