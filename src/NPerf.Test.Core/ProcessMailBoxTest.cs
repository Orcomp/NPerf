namespace NPerf.Test.Core
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentAssertions;
    using NPerf.Core.Communication;
    using NUnit.Framework;

    [TestFixture]
    public class ProcessMailBoxTest
    {
        [Test]
        public void CanSetGetMailBoxContent()
        {
            const string Content = "Content";
            using (var mailBox = new ProcessMailBox("mailBox", 1024))
            {
                mailBox.Content = Content;

                var value = mailBox.Content;

                value.ShouldBeEquivalentTo(Content);
            }
        }

        [Test]
        public void CanSyncSetGetMailBoxContent()
        {
            const int N = 10;
            const int SecondsTimeout = 30;

            var list = new List<int>();
            using (var mailBox = new ProcessMailBox("mailBox", 1024))
            {
                var setTask = Task.Factory.StartNew(() =>
                    {
                        for (var i = 0; i < N; i++)
                        {
                            mailBox.Content = i;
                        }
                    });

                var getTask = Task.Factory.StartNew(() =>
                {
                    for (var i = 0; i < N; i++)
                    {
                        list.Add((int)mailBox.Content);
                    }
                });

                setTask.Wait(TimeSpan.FromSeconds(SecondsTimeout)).Should().BeTrue();
                getTask.Wait(TimeSpan.FromSeconds(SecondsTimeout)).Should().BeTrue();
            }

            list.Count.Should().Be(N);

            for (var i = 0; i < N; i++)
            {
                list.Contains(i).Should().BeTrue();
            }
        }
    }
}
