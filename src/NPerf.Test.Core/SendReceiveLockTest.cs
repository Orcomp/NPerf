namespace NPerf.Test.Core
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NPerf.Test.Helpers;

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
                IList<int> result = new List<int>();
                helper.Receive<int>(result, TimeSpan.FromMilliseconds(1000)).Should().BeTrue();

                result.Count.Should().Be(1);
                result[0].Should().Be(Value);
            }            
        }

        [TestMethod]
        public void CanSyncSendReceive()
        {
            var buff = 0;
            const int N = 10;
            const int SecondsTimeout = 30;
            
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

            var list = new List<int>();
            var sendTask = Task.Factory.StartNew(() => helper.Send(TimeSpan.FromSeconds(SecondsTimeout)));
            var receiveTask = Task.Factory.StartNew(() => helper.Receive(list, TimeSpan.FromSeconds(SecondsTimeout)));
            sendTask.Wait();
            receiveTask.Wait();

            sendTask.Result.Should().BeTrue();
            receiveTask.Result.Should().BeTrue();

            list.Count.Should().Be(N);

            for (var i = 0; i < N; i++)
            {
                list.Contains(i).Should().BeTrue();
            }
        }
    }
}
