namespace NPerf.Test.Experiment
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NPerf.Core.Communication;
    using NPerf.Core.PerfTestResults;
    using NPerf.Experiment;

    [TestClass]
    public class TestRunnerTest
    {
        [TestMethod]
        public void CanObserveTestRunner()
        {
            var setUpCounter = 0;
            var testCounter = 0;
            var tearDownCounter = 0;
            const int Start = 0;
            const int Step = 7;
            const int End = 135;
            const string MailBoxName = "box";            

            using (var mailBox = new ProcessMailBox(MailBoxName))
            {
                using (var observer = new TestObserver(MailBoxName))
                {
                    var list = new List<PerfTestResult>();
                    var runner = new TestRunner(i => setUpCounter++, () => testCounter++, () => tearDownCounter++, i => i / 2d, Start, Step, End);

                    var taskObserve = Task.Factory.StartNew(
                        () =>
                        {
                            runner.Subscribe(observer);
                        });

                    var taskMailBox = Task.Factory.StartNew(
                        () =>
                        {
                            for (int i = Start; i < End; i += Step)
                            {
                                list.Add((PerfTestResult)mailBox.Content);
                            }
                        });

                    Task.WaitAll(new[] { taskObserve, taskMailBox }, TimeSpan.FromSeconds(20)).Should().BeTrue();

                    var count = (int)Math.Ceiling((End - Start + 1) / (double)Step);
                    setUpCounter.Should().Be(count);
                    testCounter.Should().Be(count);
                    tearDownCounter.Should().Be(count);

                    var index = 0;
                    for (var i = Start; i < End; i += Step)
                    {
                        list[index++].Descriptor.Should().Be(i / 2d); 
                    }
                }
            }
        }
    }
}
