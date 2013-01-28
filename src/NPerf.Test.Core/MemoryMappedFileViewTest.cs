namespace NPerf.Test.Core
{
    using System.IO.MemoryMappedFiles;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NPerf.Core.Communication;

    [TestClass]
    public class MemoryMappedFileViewTest
    {
        [TestMethod]
        public void CanReadWriteSerialize()
        {
            using (var mmf = MemoryMappedFile.CreateNew("test_mmf", 1024))
            {
                using (var view = new MemoryMappedFileView(mmf.CreateViewStream(0, 1024, MemoryMappedFileAccess.ReadWrite), 1024))
                {
                    const string TextToWrite = "text to serialize";
                    view.WriteSerialize(TextToWrite);

                    var readedObject = view.ReadDeserialize();
                    readedObject.Should().Be(TextToWrite);
                }
            }
        }
    }
}
