namespace NPerf.Core.Communication
{
    using System;
    using System.IO;
    using System.IO.MemoryMappedFiles;
    using System.Runtime.Serialization.Formatters.Binary;
    
    internal class MemoryMappedFileView : IDisposable
    {
        private MemoryMappedViewStream stream;
        private readonly int size;

        private bool disposed;

        internal MemoryMappedFileView(MemoryMappedViewStream stream)
        {
            this.stream = stream;
            this.size = ChannelConfiguration.Instance.ChannelSize;
        }

        private void ReadBytes(byte[] data)
        {
            this.stream.Seek(0, SeekOrigin.Begin);
            this.stream.Read(data, 0, data.Length);
        }

        private void WriteBytes(byte[] data, int offset)
        {
            this.stream.Seek(0, SeekOrigin.Begin);
            stream.Write(data, offset, data.Length);
        }

        public object ReadDeserialize()
        {
            return this.ReadDeserialize(this.size);
        }

        private object ReadDeserialize(int length)
        {
            var binaryData = new byte[length];
            this.ReadBytes(binaryData);
            var formatter = new BinaryFormatter();

            object data;
            using (var ms = new MemoryStream(binaryData, 0, length, true, true))
            {
                data = formatter.Deserialize(ms);
            }
            return data;
        }

        /// <summary>
        /// Serializes the data and writes it to the file.
        /// </summary>
        /// <param name="data">The data to serialize.</param>
        public void WriteSerialize(object data)
        {
            this.WriteSerialize(data, 0, this.size);
        }

        /// <summary>
        /// Serializes the data and writes it to the file.
        /// </summary>
        /// <param name="data">The data to serialize.</param>
        /// <param name="offset">The position in the file to start.</param>
        /// <param name="length">The buffer size in bytes.</param>
        private void WriteSerialize(object data, int offset, int length)
        {
            var formatter = new BinaryFormatter();
            var binaryData = new byte[length];
            using (var ms = new MemoryStream(binaryData, 0, length, true, true))
            {
                formatter.Serialize(ms, data);
                this.WriteBytes(binaryData, offset);
            }
        }

        protected void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                this.stream.Dispose();
            }

            this.stream = null;

            this.disposed = true;
        }

        ~MemoryMappedFileView()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
