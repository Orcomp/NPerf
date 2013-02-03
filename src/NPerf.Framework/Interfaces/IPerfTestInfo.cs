namespace NPerf.Framework.Interfaces
{
    using System;

    public interface IPerfTestInfo
    {
        Guid TestId { get; }
        /// <summary>
        /// Gets the name of test.
        /// </summary>
        string TestMethodName { get; }

        /// <summary>
        /// Gets the description of test.
        /// </summary>
        string TestDescription { get; }
    }
}
