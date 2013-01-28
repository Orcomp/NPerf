namespace NPerf.Test.Core
{
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NPerf.Core.Monitoring;

    [TestClass]
    public class DisposableScopeTest
    {
        [TestMethod]
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
