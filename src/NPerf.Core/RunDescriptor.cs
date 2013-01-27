namespace NPerf.Core
{
    //using System.Threading;

    public class RunDescriptor
    {
        public static readonly RunDescriptor Instance = new RunDescriptor();

        private RunDescriptor()
        {
            //System.Threading.Interlocked.Add()
        }

        public double Value { get; set; }
    }
}
