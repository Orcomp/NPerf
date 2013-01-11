namespace NPerf.Framework
{
    /// <summary>
    /// The performance test interface.
    /// </summary>
    public interface IPerfTest
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

        /// <summary>
        /// The test method.
        /// </summary>
        /// <param name="testedObject">
        /// Instance of tested object.
        /// </param>
        void Test(object testedObject);
    }
}
