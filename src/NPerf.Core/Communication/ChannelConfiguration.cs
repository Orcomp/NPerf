namespace NPerf.Core.Communication
{
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
