namespace NPerf.Lab.Queueing
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading;

    public class ReactiveQueue<T> : IDisposable
        where T : class
    {
        private Queue<T> queue;

        private readonly EventWaitHandle wh = new AutoResetEvent(false);

        private readonly object syncLock = new object();

        private bool disposed;

        private readonly Thread queueThread;

        /// <summary>
        /// Initializes a new instance of the ReactiveQueue class.
        /// </summary>
        public ReactiveQueue()
        {
            this.queue = new Queue<T>();
            this.queueThread = new Thread(this.Loop);
            this.queueThread.Start();
        }

        /// <summary>
        /// Enqueues the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Enqueue(T item)
        {
            lock (this.syncLock)
            {
                this.queue.Enqueue(item);
            }

            this.wh.Set();
        }

        /// <summary>
        /// Occurs when [dequeue handler].
        /// </summary>
        public event EventHandler<DequeueEventArgs<T>> ItemDequeued;

        /// <summary>
        /// Occurs when Enqueue is called.
        /// </summary>
        protected void Loop()
        {
            Thread.CurrentThread.Name = "ReactiveQueue";
            while (true)
            {
                T[] items = null;
                lock (this.syncLock)
                {
                    if (this.queue.Count > 0)
                    {
                        items = this.queue.ToArray();
                        this.queue.Clear();
                    }
                }

                if (items != null)
                {
                    foreach (var item in items)
                    {
                        if (item == null)
                        {
                            return;
                        }

                        this.ActionHandler(item);
                    }
                }
                else
                {
                    this.wh.WaitOne();
                }
            }
        }

        private void ActionHandler(T item)
        {
            if (this.ItemDequeued != null)
            {
                this.ItemDequeued(this, new DequeueEventArgs<T> { Item = item });
            }
        }


        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and 
        /// unmanaged resources; <c>false</c> to release only unmanaged 
        /// resources.</param>
        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.Enqueue(null);
                    this.queueThread.Join();
                    this.wh.Close();
                }
            }

            this.disposed = true;
        }
        
        /// <summary>
        /// Performs application-defined tasks associated with freeing, 
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="ReactiveQueue{T}"/> class. 
        /// </summary>
        ~ReactiveQueue()
        {
            this.Dispose(false);
        }

    }
}
