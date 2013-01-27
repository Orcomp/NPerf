namespace NPerf.Test.Core
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using FluentAssertions;
    using NPerf.Core.TestResults;
    using NUnit.Framework;

    [TestFixture]
    public class SerialisationTest
    {
        [Test]
        public void CanSerializeDeserializePerfResult()
        {
            var perfResult = new PerfResult
                                 {
                                     Descriptor = 0.1,
                                     TestedType = "TestedTypeName",
                                     Duration = 10.33,
                                     MemoryUsage = 100322,
                                     TestMethod = "TestMethodName",
                                     TesterType = "TesterTypeName",
                                     TestInfo =
                                         new TestInfo
                                             {
                                                 Description = "Description",
                                                 Name = "Name",
                                                 IsIgnore = true,
                                                 IgnoreMessage = "IgnoreMsg"
                                             }
                                 };

            var formatter = new BinaryFormatter();
            var binaryData = new byte[1024];
            object deserializedData;
            using (var ms = new MemoryStream(binaryData, 0, 1024, true, true))
            {
                formatter.Serialize(ms, perfResult);
                ms.Seek(0, SeekOrigin.Begin);
                deserializedData = formatter.Deserialize(ms);
            }

            deserializedData.Should().NotBeNull();
            deserializedData.Should().Be(perfResult);
        }

        [Test]
        public void CanSerializeDeserializePerfFailedResult()
        {
            var perfFailedResult = new PerfFailedResult
                                       {
                                           Descriptor = 0.1,
                                           TestedType = "TestedTypeName",
                                           ExceptionType = "extype",
                                           FullMessage = "fullmessage",
                                           Message = "message",
                                           Source = string.Empty, 
                                           TestMethod = "TestMethodName",
                                           TesterType = "TesterTypeName",
                                           TestInfo =
                                               new TestInfo
                                                   {
                                                       Description = "Description",
                                                       Name = "Name",
                                                       IsIgnore = true,
                                                       IgnoreMessage = "IgnoreMsg"
                                                   }
                                       };

            var formatter = new BinaryFormatter();
            var binaryData = new byte[1024];
            object deserializedData;
            using (var ms = new MemoryStream(binaryData, 0, 1024, true, true))
            {
                formatter.Serialize(ms, perfFailedResult);
                ms.Seek(0, SeekOrigin.Begin);
                deserializedData = formatter.Deserialize(ms);
            }

            deserializedData.Should().NotBeNull();
            deserializedData.Should().Be(perfFailedResult);
        }

        [Test]
        public void CanSerializeDeserializeExperimentError()
        {
            var perfFailedResult = new ExperimentError
            {
                TestedType = "TestedTypeName",
                TestMethod = "TestMethodName",
                TesterType = "TesterTypeName",
                ExceptionType = "extype",
                FullMessage = "fullmessage",
                Message = "message",
                Source = string.Empty, 
                TestInfo =
                    new TestInfo
                    {
                        Description = "Description",
                        Name = "Name",
                        IsIgnore = true,
                        IgnoreMessage = "IgnoreMsg"
                    }
            };

            var formatter = new BinaryFormatter();
            var binaryData = new byte[1024];
            object deserializedData;
            using (var ms = new MemoryStream(binaryData, 0, 1024, true, true))
            {
                formatter.Serialize(ms, perfFailedResult);
                ms.Seek(0, SeekOrigin.Begin);
                deserializedData = formatter.Deserialize(ms);
            }

            deserializedData.Should().NotBeNull();
            deserializedData.Should().Be(perfFailedResult);
        }

        [Test]
        public void CanSerializeDeserializeExperimentCompleted()
        {
            var experimentCompleted = new ExperimentCompleted
                                          {
                                              TestedType = "TestedTypeName",
                                              TestMethod = "TestMethodName",
                                              TesterType = "TesterTypeName",
                                              TestInfo =
                                                  new TestInfo
                                                      {
                                                          Description = "Description",
                                                          Name = "Name",
                                                          IsIgnore = true,
                                                          IgnoreMessage = "IgnoreMsg"
                                                      }
                                          };

            var formatter = new BinaryFormatter();
            var binaryData = new byte[1024];
            object deserializedData;
            using (var ms = new MemoryStream(binaryData, 0, 1024, true, true))
            {
                formatter.Serialize(ms, experimentCompleted);
                ms.Seek(0, SeekOrigin.Begin);
                deserializedData = formatter.Deserialize(ms);
            }

            deserializedData.Should().NotBeNull();
            deserializedData.Should().Be(experimentCompleted);
        }
    }
}
