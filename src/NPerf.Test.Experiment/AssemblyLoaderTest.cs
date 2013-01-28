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
            const string TypeName = "TestSuiteSample";
            const string AssemblyName = "NPerf.DevHelpers.dll";
            var instance = AssemblyLoader.CreateInstance(AssemblyName, TypeName);
            instance.Should().NotBeNull();
            instance.GetType().Name.Should().Be(TypeName);
        }
    }
}
