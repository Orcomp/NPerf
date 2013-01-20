namespace NPerf.Framework
{
    using System;

    /// <summary>
    /// Defines a method that returns a description of the current run.
    /// </summary>
    /// <include file='NPerf.Framework.Doc.xml' path='doc/remarkss/remarks[@name="PerfRunDescriptorAttribute"]'/>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class PerfRunDescriptorAttribute : Attribute
    {
    }
}
