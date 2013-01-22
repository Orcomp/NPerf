namespace NPerf.Experiment
{
    public class Descriptor
    {
        public static readonly Descriptor Instance = new Descriptor();

        private Descriptor()
        {
        }

        public double Value { get; set; }
    }
}
