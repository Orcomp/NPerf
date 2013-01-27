namespace NPerf.Test.Core
{
    using System.IO.MemoryMappedFiles;
    using FluentAssertions;
    using NPerf.Core.Communication;
    using NUnit.Framework;

    [TestFixture]
    public class MemoryMappedFileViewTest
    {
        [Test]
        public void CanReadWriteSerialize()
        {
            using (var mmf = MemoryMappedFile.CreateNew("test_mmf", 1024))
            {
                using (var view = new MemoryMappedFileView(mmf.CreateViewStream(0, 1024, MemoryMappedFileAccess.ReadWrite), 1024))
                {
                    var textToWrite = "text to serialize";
                    view.WriteSerialize(textToWrite);

                    var readedObject = view.ReadDeserialize();
                    readedObject.Should().Be(textToWrite);
                }
            }
        }
    }
}
