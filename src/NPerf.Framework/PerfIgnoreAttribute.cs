namespace NPerf.Framework
{
    using System;

    /// <summary>
    /// A custom attribute to disable tester or tests
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Class
        | AttributeTargets.Struct
        | AttributeTargets.Method,
        AllowMultiple = false)
    ]
    public class PerfIgnoreAttribute : System.Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PerfIgnoreAttribute"/> class. 
        /// Default constructor - initializes all fields to default values
        /// </summary>
        /// <param name="message">
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public PerfIgnoreAttribute(string message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            this.Message = message;
        }

        /// <summary>Gets the reason why the test is ignored</summary>
        /// <value>Explanation of the test ignore</value>
        public string Message { get; private set; }
    }
}
