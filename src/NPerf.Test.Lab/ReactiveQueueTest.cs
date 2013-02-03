namespace NPerf.Test.Lab
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NPerf.Lab.Queueing;

    [TestClass]
    public class ReactiveQueueTest
    {
        [TestMethod]
        public void CanUseReactiveQueueSynchronous()
        {
            using (var queue = new ReactiveQueue<string>())
            {
                var dict = new Dictionary<string, int>();
                queue.ItemDequeued += (sender, e) => dict.Add(e.Item, Thread.CurrentThread.ManagedThreadId);

                var tasks = new List<Task>();
                for (var i = 0; i < 1000; i++)
                {
                    var value = i;
                    tasks.Add(Task.Factory.StartNew(() => queue.Enqueue(value.ToString())));
                }

                Task.WaitAll(tasks.ToArray(), TimeSpan.FromSeconds(20)).Should().BeTrue();
              
                queue.Dispose();
                
                dict.Count.Should().Be(1000);

                for (var i = 0; i < 1000; i++)
                {
                    dict.Should().ContainKey(i.ToString());                    
                }

                dict.Select(x => x.Value).Distinct().Count().Should().Be(1);
            }
        }
    }
}
