namespace NPerf.Framework
{
    using System;

    /// <summary>
    /// Defines a test set-up method.
    /// </summary>
    /// <include file='NPerf.Framework.Doc.xml' path='doc/remarkss/remarks[@name="PerfSetUpAttribute"]'/>
    /// <include file='NPerf.Framework.Doc.xml' path='doc/examples/example[@name="PerfSetUpTearDownAttribute"]'/>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class PerfSetUpAttribute : Attribute
    {
    }
}
