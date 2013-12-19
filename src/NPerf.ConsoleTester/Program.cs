using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPerf.Lab;
using NPerf.DevHelpers;
using NPerf.Core.PerfTestResults;

namespace NPerf.ConsoleTester
{
    using NPerf.Test.Helpers;
    using System.Windows.Forms;

    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            using (var context = new ExecutionContext(new PerfTestConfiguration()))
            {
                Application.Run(context);
            }            
        }
    }
}
