using System;

namespace NPerf.Framework
{
    /// <summary>
    /// A performance tester class attribute.
    /// </summary>
    /// <include file='NPerf.Framework.Doc.xml' path='doc/remarkss/remarks[@name="PerfTesterAttribute"]'/>
    /// <include file='NPerf.Framework.Doc.xml' path='doc/examples/example[@name="PerfTesterAttribute"]'/>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct, AllowMultiple = false
        , Inherited = true)]
    public class PerfTesterAttribute : Attribute
    {
        /// <summary>
        /// Constructs the attribute
        /// </summary>
        /// <param name="testedType">target type</param>
        /// <param name="testCount">number of test runs</param>
        /// <exception cref="ArgumentNullException"><paramref name="testedType"/> is a null reference</exception>
        /// <exception cref="ArgumentException"><paramref name="testCount"/> is smaller than 1</exception>
        public PerfTesterAttribute(Type testedType, int testCount)
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

        /// <summary>Gets the Test description string</summary>
        /// <value>Test description</value>
        public string Description { get; set; }

        /// <summary>Gets the tested feature description string</summary>
        /// <value>Tested feature description</value>
        public string FeatureDescription { get; set; }
    }
}
