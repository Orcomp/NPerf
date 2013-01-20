namespace NPerf.Framework
{
    using System;

    /// <summary>
    /// Defines a benchmark test method.
    /// </summary>
    /// <include file='NPerf.Framework.Doc.xml' path='doc/remarkss/remarks[@name="PerfTestAttribute"]'/>
    /// <include file='NPerf.Framework.Doc.xml' path='doc/examples/example[@name="PerfTestAttribute"]'/>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class PerfTestAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PerfTestAttribute"/> class. 
        /// Constructor with test descrition
        /// </summary>
        /// <param name="description">
        /// The description.
        /// </param>
        /// <remarks>
        /// Constructs the attribute with <paramref name="description"/>.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// description is a null reference
        /// </exception>
        public PerfTestAttribute(string description = null)
        {
            if (description == null)
            {
                throw new ArgumentNullException("description");
            }

            this.Description = description;
        }

        /// <summary>Gets the Test description string</summary>
        /// <value>Test description</value>
        public string Description { get; private set; }
    }
}
