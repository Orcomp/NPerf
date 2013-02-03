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
        static void Main(string[] args)
        {
            
            Application.Run(new ExecutionContext());            
        }
    }
}
