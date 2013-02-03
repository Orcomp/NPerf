using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NPerf.Lab;

namespace NPerf.Test.Lab
{
    using NPerf.Core.Communication;
    using NPerf.DevHelpers;
    using NPerf.Test.Helpers;

    [TestClass]
    public class ExperimentProcessTest
    {
        [TestMethod]
        public void CanExecuteExperiment()
        {
            const string ChannelName = "channel";
            var experiment = new ExperimentProcess(
                ChannelName,
                typeof(PerfTestSample).Assembly.Location,
                typeof(PerfTestSuiteSample).Name,
                typeof(TestedObject),
                "TestMethod");
            using (var mailBox = new ProcessMailBox(ChannelName))
            {
                experiment.Start();
                var val = mailBox.Content;
            }
        }
    }
}
