namespace NPerf.Test.Experiment
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NPerf.Core.Communication;
    using NPerf.Core.PerfTestResults;
    using NPerf.Experiment;
    using NPerf.Test.Helpers;

    [TestClass]
    public class TestObserverTest
    {
        [TestMethod]
        public void CanUseTestObserver()
        {
            const string MailBoxName = "box";
            const string ErrorText = "error";
            using (var mailBox = new ProcessMailBox(MailBoxName, TimeSpan.FromMilliseconds(-1)))
            {
                using (var observer = new TestObserver(MailBoxName))
                {
                    var list = new List<object>();

                    var taskObserve = Task.Factory.StartNew(
                        () =>
                            {
                                observer.OnNext(PerTestResultGenerator.CreatePerfResult());
                                observer.OnError(new Exception(ErrorText));
                                observer.OnCompleted();
                            });

                    var taskMailBox = Task.Factory.StartNew(
                        () =>
                            { 
                                list.Add(mailBox.Content);
                                list.Add(mailBox.Content);
                                list.Add(mailBox.Content);
                            });

                    Task.WaitAll(new[] { taskObserve, taskMailBox }, TimeSpan.FromSeconds(20)).Should().BeTrue();
                    list.Count.Should().Be(3);
                    list[0].Should().Be(PerTestResultGenerator.CreatePerfResult());
                    list[1].Should().Be(new ExperimentError(new Exception(ErrorText)));
                    list[2].Should().Be(new ExperimentCompleted());
                }
            }
        }
    }
}
