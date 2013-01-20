namespace NPerf.Framework.Interfaces
{
    /// <summary>
    /// The the performance fixture that should be run against instances of a given interface implementations.
    /// </summary>
    public interface IPerfTestSuite : IPerfTestSuiteInfo
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
