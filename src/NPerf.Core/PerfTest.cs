namespace NPerf.Core
{
    using System;
    using System.Reflection;
    using System.Xml;
    using System.Xml.Serialization;

    using Fasterflect;

    using NPerf.Core.Collections;
    using NPerf.Framework;

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
            this.Runs = new PerfTestRunCollection();
            this.IsIgnored = false;
            this.IgnoredMessage = null;
        }

        public PerfTest(MethodInfo mi)
        {
            if (mi == null) throw new ArgumentNullException("mi");

            this.Name = mi.Name;
            this.Runs = new PerfTestRunCollection();

            this.IsIgnored = mi.HasAttribute<PerfIgnoreAttribute>();
            if (this.IsIgnored)
            {
                var attr = mi.Attribute<PerfIgnoreAttribute>();
                this.IgnoredMessage = attr.Message;
            }
            else this.IgnoredMessage = null;
        }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("ignored")]
        public bool IsIgnored { get; set; }

        [XmlAttribute("ignored-message")]
        public string IgnoredMessage { get; set; }

        [XmlArrayItem(ElementName = "run", Type = typeof(PerfTestRun))]
        [XmlArray(ElementName = "runs")]
        public PerfTestRunCollection Runs { get; set; }
    }
}
