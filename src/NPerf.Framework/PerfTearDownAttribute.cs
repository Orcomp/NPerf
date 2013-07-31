namespace NPerf.Framework
{
    using System;

    /// <summary>
    /// Defines a test tear-down method.
    /// </summary>
    /// <include file='NPerf.Framework.Doc.xml' path='doc/remarkss/remarks[@name="PerfTearDownAttribute"]'/>
    /// <include file='NPerf.Framework.Doc.xml' path='doc/examples/example[@name="PerfSetUpTearDownAttribute"]'/>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class PerfTearDownAttribute : Attribute
    {
    }
}
