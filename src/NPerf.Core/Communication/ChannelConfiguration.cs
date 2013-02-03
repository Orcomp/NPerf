namespace NPerf.Core.Communication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    internal class ChannelConfiguration
    {
        private static readonly ChannelConfiguration instance = new ChannelConfiguration();

        private ChannelConfiguration()
        {
        }

        public static ChannelConfiguration Instance
        {
            get
            {
                return instance;
            }
        }

        public int ChannelSize
        {
            get
            {
                return 1024 * 5;
            }
        }
    }
}
