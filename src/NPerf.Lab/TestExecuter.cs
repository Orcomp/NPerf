using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Reflection;

namespace NPerf.Lab
{
    public class TestExecuter : ITestExecuter
    {
        private Process experimentProcess;

        public TestExecuter(Assembly suiteAssembly, Type testedType, int testIndex)
        {
            this.experimentProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = false,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    FileName =
                        Environment.CurrentDirectory
                        + "\\NPerf.Experiment.exe",
                    Arguments = string.Format("-ta {0} -ft {1} -ti {2} -ra {3} -st {4}", 
                        suiteAssembly.Location, 
                        "GeneratedTestSuite",
                        testIndex,
                        testedType.Assembly.Location,
                        testedType.Name)
                }
            };
        }

        public void ExecuteTest()
        {            
            this.experimentProcess.Start();
        }
    }
}
