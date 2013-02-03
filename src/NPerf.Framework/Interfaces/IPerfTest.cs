namespace NPerf.Framework.Interfaces
{
    /// <summary>
    /// The performance test interface.
    /// </summary>
    public interface IPerfTest : IPerfTestInfo
    {
        /// <summary>
        /// The test method.
        /// </summary>
        /// <param name="testedObject">
        /// Instance of tested object.
        /// </param>
        void Test(object testedObject);
    }
}
