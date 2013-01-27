using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPerf.Lab.Threading
{
    /// <summary>
    /// Interface for simple runnable management.
    /// </summary>
    internal interface IRunnable : IDisposable
    {
        /// <summary>Start the runnable.</summary>
        void Start();
        /// <summary>Stop the runnable. This method tells the thread to finish it's work and (depending on implementation) waits until the thread really finishes.</summary>
        void Stop();
        /// <summary>Stop the runnable. This method forces the threads to abort and propably leaves data in an invalid state. It won't wait until the thread really aborts.</summary>
        void ForceAbort();
    }
}
