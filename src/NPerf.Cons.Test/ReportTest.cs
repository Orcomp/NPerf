using System;

namespace NPerf.Cons.Test
{
    public class ReportTest
    {
        [STAThread]
        public static void Main(string[] args)
        {
            new NPerf.Cons.ReportGenerator().GenerateReportWithType(typeof(IDateSorter));
            Console.WriteLine("Report generated from inner class...");
            Console.ReadLine();
        }
    }
}
