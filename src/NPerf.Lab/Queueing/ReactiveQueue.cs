namespace NPerf.Lab.Queueing
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading;

    public class ReactiveQueue<T> : IDisposable
    {
        private ConcurrentQueue<T> queue;

        private bool closed;

        private object syncLock = new object();

        private readonly object closeLock = new object();

        private bool disposed;

        /// <summary>
        /// Gets or sets a value indicating whether this <see 
        /// cref="ReactiveQueue&lt;T&gt;"/> is closed.
        /// </summary>
        /// <value><c>true</c> if closed; otherwise, <c>false</c>.</value>
        public bool Closed
        {
            get
            {
                lock (this.closeLock)
                {
                    return this.closed;
                }
            }

            set
            {
                lock (this.closeLock)
                {
                    this.closed = value;
                }
            }
        }
      
        /// <summary>
        /// Gets or sets the sync lock.
        /// </summary>
        /// <value>The sync lock.</value>
        internal object SyncLock
        {
            get { return this.syncLock; }
            set { this.syncLock = value; }
        }

        /// <summary>
        /// Initializes a new instance of the ReactiveQueue class.
        /// </summary>
        public ReactiveQueue()
        {
            this.queue = new ConcurrentQueue<T>();
            ThreadPool.QueueUserWorkItem(this.OnEnqueue);
        }

        /// <summary>
        /// Enqueues the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Enqueue(T item)
        {
            if (this.Closed)
            {
                return;
            }

            lock (this.syncLock)
            {
                this.queue.Enqueue(item);
                Monitor.PulseAll(this.syncLock);
            }
        }

        /// <summary>
        /// Occurs when [dequeue handler].
        /// </summary>
        public event EventHandler<DequeueEventArgs<T>> ItemDequeued;

        /// <summary>
        /// Occurs when Enqueue is called.
        /// </summary>
        protected void OnEnqueue(object obj)
        {
            Thread.CurrentThread.Name = "ReactiveQueue";
            while (!this.Closed)
            {
                var items = new List<T>();
                lock (this.syncLock)
                {
                    if (this.queue.Count == 0)
                    {
                        Monitor.Wait(this.syncLock);
                    }

                    if (this.ItemDequeued != null)
                    {
                        while (this.queue.Count > 0)
                        {
                            T item;
                            if (this.queue.TryDequeue(out item))
                            {
                                items.Add(item);
                            }
                        }
                    }
                }

                if (this.ItemDequeued != null && items.Count > 0)
                {
                    items.ForEach(this.ActionHandler);
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
        /// Closes this instance.
        /// </summary>
        public void Close()
        {
            this.Dispose();
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
                    this.Closed = true;
                    lock (this.syncLock)
                    {
                        Monitor.PulseAll(this.syncLock);
                    }
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
