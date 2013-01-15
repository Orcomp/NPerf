namespace NPerf.Framework
{
    using System;

    /// <summary>
    /// A performance tester class attribute.
    /// </summary>
    /// <include file='NPerf.Framework.Doc.xml' path='doc/remarkss/remarks[@name="PerfTesterAttribute"]'/>
    /// <include file='NPerf.Framework.Doc.xml' path='doc/examples/example[@name="PerfTesterAttribute"]'/>
    [AttributeUsage(AttributeTargets.Class 
        | AttributeTargets.Interface 
        | AttributeTargets.Struct, AllowMultiple = false, 
        Inherited = true)]
    public class PerfTesterAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PerfTesterAttribute"/> class. 
        /// Constructs the attribute
        /// </summary>
        /// <param name="testedType">
        /// target type
        /// </param>
        /// <param name="testCount">
        /// number of test runs
        /// </param>
        /// <param name="description">
        /// Test description.
        /// </param>
        /// <param name="featureDescription">
        /// Tested feature description.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="testedType"/> is a null reference
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="testCount"/> is smaller than 1
        /// </exception>
        public PerfTesterAttribute(Type testedType, int testCount, string description = null, string featureDescription = null)
        {
            if (testedType == null)
            {
                throw new ArgumentNullException("testedType");
            }

            if (testCount < 1)
            {
                throw new ArgumentException("test count is smaller that 1");
            }

            this.TestedType = testedType;
            this.TestCount = testCount;
            this.Description = description;
            this.FeatureDescription = featureDescription;
        }

        /// <summary>
        /// Gets the target type of the tester
        /// </summary>
        /// <value>
        /// Target type of the tester
        /// </value>
        public Type TestedType { get; private set; }

        /// <summary>
        /// Gets the number of test runs
        /// </summary>
        /// <value>
        /// The number of test runs 
        /// </value>
        public int TestCount { get; private set; }

        /// <summary>
        /// Gets the the Test description string.
        /// </summary>
        /// <value>Test description</value>
        public string Description { get; private set; }

        /// <summary>
        /// Gets the tested feature description string
        /// </summary>
        /// <value>Tested feature description</value>
        public string FeatureDescription { get; private set; }
    }
}
