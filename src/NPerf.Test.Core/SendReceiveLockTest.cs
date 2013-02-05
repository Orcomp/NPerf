namespace NPerf.Test.Core
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NPerf.Test.Helpers;
    using System.Collections.Concurrent;

    [TestClass]
    public class SendReceiveLockTest
    {
        const string FullName = "full";

        const string EmptyName = "empty";

        [TestMethod]
        public void CandExecuteSending()
        {
            var list = new List<int>();
            const int Value = 1;

            using (var helper = new SendReceiveLockHelper(FullName, EmptyName))
            {
                helper.AddSendAction(() => list.Add(Value));
                helper.Send(TimeSpan.FromMilliseconds(1000)).Should().BeTrue();
            }

            list.Count.Should().Be(1);
            list[0].Should().Be(Value);
        }

        [TestMethod]
        public void CandExecuteReceiving()
        {           
            const int Value = 1;
            var list = new List<int>();

            using (var helper = new SendReceiveLockHelper(FullName, EmptyName))
            {
                helper.AddSendAction(() => list.Add(Value));
                helper.Send(TimeSpan.FromMilliseconds(1000)).Should().BeTrue();

                helper.AddReceiveFunction(() => list[0]);
                var result = new ConcurrentBag<int>();
                helper.Receive<int>(result, TimeSpan.FromMilliseconds(1000)).Should().BeTrue();

                result.Count.Should().Be(1);
                result.ToArray()[0].Should().Be(Value);
            }            
        }

        [TestMethod]
        public void CanSyncSendReceive()
        {
            var buff = 0;
            const int N = 20;
            const int SecondsTimeout = 60;
            
            var helper = new SendReceiveLockHelper(FullName, EmptyName);
            for (var i = 0; i < N; i++)
            {
                var value = i;
                helper.AddSendAction(
                    () =>
                        {
                            using (new SingleThreadAccessController())
                            {
                                buff = value;
                            }
                        });

                helper.AddReceiveFunction(() =>
                    {
                        using (new SingleThreadAccessController())
                        {
                            return buff;
                        }
                    });
            }

            var list = new ConcurrentBag<int>();
            var sendTask = Task.Factory.StartNew(() => helper.Send(TimeSpan.FromSeconds(SecondsTimeout)));
            var receiveTask = Task.Factory.StartNew(() => helper.Receive(list, TimeSpan.FromSeconds(SecondsTimeout)));

            Task.WaitAll(sendTask, receiveTask);

            sendTask.Result.Should().BeTrue("sendTask.Result should be true");
            receiveTask.Result.Should().BeTrue("receiveTask.Result should be true");

            list.Count.Should().Be(N, string.Format("list.Count should be equals {0}", N));

            for (var i = 0; i < N; i++)
            {
                list.Should().Contain(i, string.Format("list should contains {0}", i));
            }
        }
    }
}
