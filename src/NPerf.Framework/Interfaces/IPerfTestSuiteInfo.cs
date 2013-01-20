namespace NPerf.Framework.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IPerfTestSuiteInfo
    {        
        /// <summary>
        /// Gets the number of each test runs.
        /// </summary>
        int DefaultTestCount { get; }

        /// <summary>
        /// Gets the description string.
        /// </summary>
        /// <value>Test description.</value>
        string Description { get; }

        /// <summary>
        /// Gets the tested feature description string.
        /// </summary>
        /// <value>Tested feature description</value>
        string FeatureDescription { get; }
    }
}
