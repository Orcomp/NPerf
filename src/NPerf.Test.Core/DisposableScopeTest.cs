namespace NPerf.Test.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using NPerf.Core.Monitoring;

    using NUnit.Framework;
    using FluentAssertions;

    [TestFixture]
    public class DisposableScopeTest
    {
        [Test]
        public void CanUseDisposableScope()
        {
            bool? disposed = null;
            using (new DisposableScope(() => disposed = false, () => disposed = true))
            {
                disposed.Should().BeFalse();
            }

            disposed.Should().BeTrue();
        }
    }
}
