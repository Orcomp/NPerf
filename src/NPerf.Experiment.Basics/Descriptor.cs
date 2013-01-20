namespace NPerf.Experiment.Basics
{
    public class Descriptor
    {
        public static readonly  Descriptor Instance = new Descriptor();

        private Descriptor()
        {
        }

        public long Value { get; set; }
    }
}
