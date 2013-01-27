namespace NPerf.Lab.Queueing
{
    using System;

    public class DequeueEventArgs<T> : EventArgs
    {
        public T Item { get; set; }
    }
}
