namespace NPerf.Core
{
    public class RunDescriptor
    {
        public static readonly RunDescriptor Instance = new RunDescriptor();

        private RunDescriptor()
        {
        }

        public double Value { get; set; }
    }
}
