using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Diagnostics;
using NPerf.DevHelpers;
using NPerf.Builder;

namespace NPerf.TestLabs.Console
{
    [DisplayName("")]
    [Description("")]
    public sealed class ExecuteLab : BaseConsoleLab
    {
        private Process experimentProcess;

        public ExecuteLab()
        {
            this.WaitingForUserInput = true;

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
                         + "\\NPerf.Experiment.exe"
                 }
            };
        }

        [DisplayName("Run all experiments")]
        protected override void Main()
        {
            this.RunExperiments();
        }

        [DisplayName("NPerf.Experiment -ta Perf.DevHelpers -ra TestSuiteSample -ti 0 -ra Perf.DevHelpers -st TestedObject")]
        [Description("")]
        public void DefaultExperiment()
        {                        
            this.TraceLine("Enter arguments to continue. For axample: ");
            this.TraceLine(
                string.Format(
                    "-ta {0} -ft {1} -ti 0 -ra {2} -st {3}",
                    typeof(TestSuiteSample).Assembly.Location,
                    typeof(TestSuiteSample).Name,
                    typeof(TestSuiteSample).Assembly.Location,
                    typeof(TestedObject).Name));
            this.TraceLine();
            this.experimentProcess.StartInfo.Arguments = this.UserInput();
            this.experimentProcess.Start();
            this.experimentProcess.WaitForExit();
            var error = this.experimentProcess.StandardError.ReadToEnd();
           // var output = this.experimentProcess.StandardOutput.ReadToEnd();
            this.TraceStatus("Outputs:");
          //  this.TraceLine(output);
            this.TraceStatus("Errors:");
            this.TraceLine(error);
            this.TraceSuccess();
        }

        public void BuldAndRun()
        {
            var builder = new TestSuiteAssemblyBuilder(typeof(AttribitedFixtureSample), typeof(TestedObject));
            var assm = builder.CreateTestSuite();

            this.experimentProcess.StartInfo.Arguments = string.Format(
                    "-ta {0} -ft {1} -ti 0 -ra {2} -st {3}",
                    assm,
                    "GeneratedTestSuite",
                    typeof(TestSuiteSample).Assembly.Location,
                    typeof(TestedObject).Name);
            this.experimentProcess.Start();
//
            var error = this.experimentProcess.StandardError.ReadToEnd();
          //  var output = this.experimentProcess.StandardOutput.ReadToEnd();
            this.experimentProcess.WaitForExit();
            this.TraceStatus("Outputs:");
         //   this.TraceLine(output);
            this.TraceStatus("Errors:");
            this.TraceLine(error);
            this.TraceSuccess();
        }
    }
}