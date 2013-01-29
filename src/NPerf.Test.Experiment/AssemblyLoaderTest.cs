namespace NPerf.Test.Experiment
{
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NPerf.Experiment;

    [TestClass]
    public class AssemblyLoaderTest
    {
        [TestMethod]
        public void CanLoadType()
        {
            string TypeName = typeof(NPerf.DevHelpers.PerfTestSuiteSample).Name;
            string AssemblyName = typeof(NPerf.DevHelpers.PerfTestSuiteSample).Assembly.Location;
            var instance = AssemblyLoader.CreateInstance(AssemblyName, TypeName);
            instance.Should().NotBeNull();
            instance.GetType().Name.Should().Be(TypeName);
        }
    }
}
