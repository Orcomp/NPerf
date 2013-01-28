namespace NPerf.Test.Core
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NPerf.Core.Monitoring;

    [TestClass]
    public class MonitorTest
    {
        [TestMethod]
        public void CanUseDurationMonitor()
        {
            var monitor = new DurationMonitor();

            monitor.Value.Should().Be(0);
            using (monitor.Observe())
            {
            }

            var value1 = monitor.Value;
            value1.Should().NotBe(0);

            using (monitor.Observe())
            {
                Thread.Sleep(20);
            }

            var value2 = monitor.Value;
            value2.Should().NotBe(0);

            (value1 < value2).Should().BeTrue();
        }

        [TestMethod]
        public void CanUseMemoryMonitor()
        {
            var monitor = new MemoryMonitor();

            monitor.Value.Should().Be(0);

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            using (monitor.Observe())
            {
            }

            var value1 = monitor.Value;
            value1.Should().NotBe(0);

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            using (monitor.Observe())
            {
                var list = new List<long>();
                for (var i = 0L; i < 10000L; i++)
                {
                    list.Add(i);
                }
            }

            var value2 = monitor.Value;
            value2.Should().NotBe(0);

            (value1 < value2).Should().BeTrue();
        }
    }
}
