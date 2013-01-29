namespace NPerf.Test.Experiment
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NPerf.Core.Communication;
    using NPerf.Core.PerfTestResults;
    using NPerf.DevHelpers;
    using NPerf.Experiment;

    [TestClass]
    public class ExperimentTest
    {
        [TestMethod]
        public void CanExecuteExperiment()
        {
            var parameters = new SartParameters
            {
                ChannelName = "channelName", 
                SuiteAssembly = typeof(NPerf.DevHelpers.PerfTestSuiteSample).Assembly.Location,
                SuiteType = typeof(NPerf.DevHelpers.PerfTestSuiteSample).Name,
                SubjectAssembly = typeof(NPerf.DevHelpers.PerfTestSuiteSample).Assembly.Location, 
                SubjectType = typeof(TestedObject).Name, TestMethod = "TestMethod", 
                Start = "0", 
                Step = "1", 
                End = "10"
            };

            var start = int.Parse(parameters.Start);
            var step = int.Parse(parameters.Step);
            var end = int.Parse(parameters.End);
            using (var mailBox = new ProcessMailBox(parameters.ChannelName, 1024))
            {
                var list = new List<TestResult>();
                var experimentTask = Task.Factory.StartNew(() => ExperimentScope.Start(parameters));

                var taskMailBox = Task.Factory.StartNew(
                    () =>
                        {
                            for (var i = start; i < end; i += step)
                            {
                                list.Add((TestResult)mailBox.Content);
                            }
                        });

                Task.WaitAll(new[] { experimentTask, taskMailBox }, TimeSpan.FromSeconds(20)).Should().BeTrue();

                list.Count.Should().Be(10);
            }
        }
    }
}
