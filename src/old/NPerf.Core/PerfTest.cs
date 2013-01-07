namespace NPerf.Core
{
    using System;
    using System.Xml;
    using System.Xml.Serialization;
    using Fasterflect;
    using NPerf.Framework;
    using System.Collections.Generic;

    /// <summary>
    /// Summary description for PerfTest.
    /// </summary>
    [Serializable]
    [XmlRoot("test")]
    public class PerfTest
    {
        public PerfTest()
        {
            this.Name = null;
            this.Runs = new List<PerfTestRun>();
            this.IsIgnored = false;
            this.IgnoredMessage = null;
        }

        public PerfTest(System.Reflection.MethodInfo mi)
        {
            if (mi == null)
            {
                throw new ArgumentNullException("mi");
            }

            this.Name = mi.Name;
            this.Runs = new List<PerfTestRun>();

            this.IsIgnored = mi.HasAttribute<PerfIgnoreAttribute>();
            this.IgnoredMessage = this.IsIgnored ? mi.Attribute<PerfIgnoreAttribute>().Message : null;
        }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("ignored")]
        public bool IsIgnored { get; set; }

        [XmlAttribute("ignored-message")]
        public string IgnoredMessage { get; set; }

        [XmlArrayItem(ElementName = "run", Type = typeof(PerfTestRun))]
        [XmlArray(ElementName = "runs")]
        public IList<PerfTestRun> Runs { get; set; }
    }
}
