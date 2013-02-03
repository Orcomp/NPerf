namespace NPerf.Framework.Interfaces
{
    using System;

    public interface IPerfTestSuiteInfo
    {
        //Guid TestSuiteId { get; }

        Type TestedType { get; }

        Type TestedAbstraction { get; }

        /// <summary>
        /// Gets the number of each test runs.
        /// </summary>
        int DefaultTestCount { get; }

        /// <summary>
        /// Gets the description string.
        /// </summary>
        /// <value>Test description.</value>
        string TestSuiteDescription { get; }

        /// <summary>
        /// Gets the tested feature description string.
        /// </summary>
        /// <value>Tested feature description</value>
        string FeatureDescription { get; }
    }
}
