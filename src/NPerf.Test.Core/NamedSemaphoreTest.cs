namespace NPerf.Test.Core
{
    using System.Collections.Generic;
    using System.Threading;
    using FluentAssertions;
    using NPerf.Core.Communication;
    using NUnit.Framework;

    [TestFixture]
    class NamedSemaphoreTest
    {
        [Test]
        public void CanCreateNamedSemaphore()
        {
            using (var semaphore = NamedSemaphore.OpenOrCreate("Named_semaphore", 2, 3))
            {
                semaphore.Should().NotBeNull();
                using (var semaphore2 = NamedSemaphore.OpenOrCreate("Named_semaphore", 2, 3))
                {
                    semaphore2.Should().NotBeNull();
                }
            }
        }

        [Test]
        public void CanUseNamedSemaphores()
        {
            var list = new List<int>();

            const string SemaphoreName = "Named semaphore";
            
            var thread1 = new Thread(
                () =>
                {
                    Thread.Sleep(100);
                    using (var semaphore = NamedSemaphore.OpenOrCreate(SemaphoreName, 1, 1))
                    {
                        semaphore.WaitOne();
                        list.Add(2);
                        semaphore.Release();
                    }
                });

            var thread2 = new Thread(
                () =>
                {
                    using (var semaphore = NamedSemaphore.OpenOrCreate(SemaphoreName, 1, 1))
                    {
                        semaphore.WaitOne();
                        list.Add(1);
                        Thread.Sleep(1000);
                        semaphore.Release();
                        
                        Thread.Sleep(10);
                        semaphore.WaitOne();
                        list.Add(3);
                        semaphore.Release();
                    }
                });
          
            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();

            list.Count.Should().Be(3);
            list[0].Should().Be(1);
            list[1].Should().Be(2);
            list[2].Should().Be(3);
        }
    }
}
