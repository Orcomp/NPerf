namespace NPerf.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IPerfTestInfo
    {
        /// <summary>
        /// Gets the name of test.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the description of test.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets a value indicating whether the test should be ignored.
        /// </summary>
        bool IsIgnore { get; }

        /// <summary>
        /// Gets the ignore message.
        /// </summary>
        string IgnoreMessage { get; }
    }
}
