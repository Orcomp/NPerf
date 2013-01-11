namespace NPerf.Framework
{
    /// <summary>
    /// The the performance fixture that should be run against instances of a given interface implementations.
    /// </summary>
    public interface IPerfFixture
    {
        /// <summary>
        /// Gets performance tests.
        /// </summary>
        /// <value></value>
        IPerfTest[] Tests { get; }

        /// <summary>
        /// Gets testing logger.
        /// </summary>
        IPerfLogger Logger { get; }

        /// <summary>
        /// Gets performance counter monitors.
        /// </summary>
        IPerfMonitor[] Monitors { get; }

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
        
        /// <summary>
        /// Gets or sets the sequence number of the current test.
        /// </summary>
        int CurrentIteration { get; set; }

        /// <summary>
        /// The test set up method.
        /// </summary>
        /// <param name="iteration">
        /// Test iteration.
        /// </param>
        /// <param name="testedObject">
        /// The tested object.
        /// </param>
        void SetUp(int iteration, object testedObject);

        /// <summary>
        /// The tear down.
        /// </summary>
        /// <param name="testedObject">
        /// The tested object.
        /// </param>
        void TearDown(object testedObject);        
    }
}
