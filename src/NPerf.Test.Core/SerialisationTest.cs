namespace NPerf.Test.Core
{
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NPerf.Test.Helpers;

    [TestClass]
    public class SerialisationTest
    {
        [TestMethod]
        public void CanSerializeDeserializePerfResult()
        {
            var perfResult = TestResultGenerator.CreatePerfResult();

            var formatter = new BinaryFormatter();
            var binaryData = new byte[1024 * 5];
            object deserializedData;
            using (var ms = new MemoryStream(binaryData, 0, 1024 * 5, true, true))
            {
                formatter.Serialize(ms, perfResult);
                ms.Seek(0, SeekOrigin.Begin);
                deserializedData = formatter.Deserialize(ms);
            }

            deserializedData.Should().NotBeNull();
            deserializedData.Should().Be(perfResult);
            deserializedData.Should().NotBeSameAs(perfResult);
        }

        [TestMethod]
        public void CanSerializeDeserializeExperimentError()
        {
            var perfFailedResult = TestResultGenerator.CreateExperimentError();

            var formatter = new BinaryFormatter();
            var binaryData = new byte[1024 * 5];
            object deserializedData;
            using (var ms = new MemoryStream(binaryData, 0, 1024 * 5, true, true))
            {
                formatter.Serialize(ms, perfFailedResult);
                ms.Seek(0, SeekOrigin.Begin);
                deserializedData = formatter.Deserialize(ms);
            }

            deserializedData.Should().NotBeNull();
            deserializedData.Should().Be(perfFailedResult);
            deserializedData.Should().NotBeSameAs(perfFailedResult);
        }

        [TestMethod]
        public void CanSerializeDeserializeExperimentCompleted()
        {
            var experimentCompleted = TestResultGenerator.CreateExperimentCompleted();

            var formatter = new BinaryFormatter();
            var binaryData = new byte[1024 * 5];
            object deserializedData;
            using (var ms = new MemoryStream(binaryData, 0, 1024 * 5, true, true))
            {
                formatter.Serialize(ms, experimentCompleted);
                ms.Seek(0, SeekOrigin.Begin);
                deserializedData = formatter.Deserialize(ms);
            }

            deserializedData.Should().NotBeNull();
            deserializedData.Should().Be(experimentCompleted);
            deserializedData.Should().NotBeSameAs(experimentCompleted);
        }
    }
}
