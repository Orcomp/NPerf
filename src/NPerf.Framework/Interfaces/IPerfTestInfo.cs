namespace NPerf.Framework.Interfaces
{
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
    }
}
