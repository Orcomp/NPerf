namespace NPerf.Core.Communication
{
    using System;
    using System.IO.MemoryMappedFiles;

    /// <summary>
    /// ProcessMailBox is an Inter-Process mailbox.
    /// A mailbox is a blocking single item container.
    /// </summary>
    /// <remarks>All members of this class are thread-safe. </remarks>
    public sealed class ProcessMailBox : IDisposable
    {
        private MemoryMappedFile file;

        private MemoryMappedFileView view;

        private SendReceiveLock mailBoxSync;

        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessMailBox"/> class.
        /// </summary>
        /// <param name="name">
        /// The name for the semaphores and the shared memory file.
        /// </param>
        /// <param name="size">
        /// The size of the shared memory in terms of bytes.
        /// </param>
        public ProcessMailBox(string name, int size)
        {
            this.mailBoxSync = new SendReceiveLock(name + ".FullMutex.MailBox", name + ".EmptyMutex.MailBox");
            this.file = MemoryMappedFile.CreateOrOpen(name + ".MemoryMappedFile.MailBox", size, MemoryMappedFileAccess.ReadWrite);
            this.view = new MemoryMappedFileView(this.file.CreateViewStream(0, size, MemoryMappedFileAccess.ReadWrite), size);
        }

        /// <summary>
        /// Gets or sets the content content accessor. Blocking on getting if empty and on setting if full.
        /// </summary>
        /// <remarks>This member is thread-safe.</remarks>
        public object Content
        {
            get
            {
                return this.mailBoxSync.Receive(() => this.view.ReadDeserialize());
            }

            set
            {
                this.mailBoxSync.Send(() => this.view.WriteSerialize(value));
            }
        }

        private void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                this.view.Dispose();
                this.file.Dispose();
                this.mailBoxSync.Dispose();
            }

            this.view = null;
            this.file = null;
            this.mailBoxSync = null;

            this.disposed = true;
        }

        ~ProcessMailBox()
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
