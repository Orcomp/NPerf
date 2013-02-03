namespace NPerf.Lab.Queueing
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading;

    public class ReactiveQueue<T> : IDisposable
    {
        private Queue<T> queue;

        private bool closed;

        private bool closing;

        private readonly object syncLock = new object();

        private bool disposed;

        private readonly Thread queueThread;

        /// <summary>
        /// Gets or sets a value indicating whether this <see 
        /// cref="ReactiveQueue&lt;T&gt;"/> is closed.
        /// </summary>
        /// <value><c>true</c> if closed; otherwise, <c>false</c>.</value>
        public bool Closed
        {
            get
            {
                lock (this.syncLock)
                {
                    return this.closed;
                }
            }

            set
            {

                if (value)
                {
                    lock (this.syncLock)
                    {
                        this.closing = true;
                        Monitor.PulseAll(this.syncLock);
                    }
                    
                    this.queueThread.Join();
                }
                else
                {
                    lock (this.syncLock)
                    {
                        this.closing = false;
                        this.closed = false;
                    }
                    this.queueThread.Start();
                }
            }
        }


        /// <summary>
        /// Initializes a new instance of the ReactiveQueue class.
        /// </summary>
        public ReactiveQueue()
        {
            this.queue = new Queue<T>();
            this.queueThread = new Thread(this.Loop);
            this.queueThread.Start();
            //ThreadPool.QueueUserWorkItem(this.OnEnqueue);
        }

        /// <summary>
        /// Enqueues the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Enqueue(T item)
        {
            lock (this.syncLock)
            {
                if (this.Closed || this.closing)
                {
                    return;
                }

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
        protected void Loop()
        {
            Thread.CurrentThread.Name = "ReactiveQueue";
            while (!this.Closed)
            {
                T[] items = null;
                lock (this.syncLock)
                {
                    if (this.queue.Count == 0 && !this.closed)
                    {
                        Monitor.Wait(this.syncLock);
                    }

                    if (this.ItemDequeued != null)
                    {
                        items = this.queue.ToArray();
                        this.queue.Clear();
                    }

                    if (this.closing)
                    {
                        this.closed = true;
                    }
                }

                if (this.ItemDequeued != null && items != null && items.Length > 0)
                {
                    Array.ForEach(items, this.ActionHandler);
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
