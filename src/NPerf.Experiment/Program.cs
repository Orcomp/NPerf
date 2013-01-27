namespace NPerf.Experiment
{
    using System;
    using NPerf.Core;
    using NPerf.Core.Communication;
    using NPerf.Core.TestResults;
    using NPerf.Framework.Interfaces;

    internal class Program
    {
        private static string boxName;

        public static void Main(string[] args)
        {
            /* NPerf.Experiment -box boxName -ta toolAssembly -ft testSuiteTypeName -ti testIndex -ra researchedAssebmly -st subjectType -start startValue -step stepValue -end andValue
             * 
             * toolAssembly - file name of the assemly with test suites
             * testSuiteTypeName - the name of type, which implements IPerfTestSuite interface
             * testIndex - index of test method in executed test suite
             */

            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;
            RunDescriptor.Instance.Value = -1;
            var arguments = args.ConvertToArguments();
            boxName = arguments.ExtractValue("box");

            if (string.IsNullOrEmpty(boxName))
            {
                SendError(new Exception("Mailbox name for interprocess communication was not set."));
                return;
            }

            using (var testObserver = new TestObserver(boxName))
            {
                try
                {
                    var toolAssemblyName = arguments.ExtractValue("ta");
                    var testSuiteTypeName = arguments.ExtractValue("ft");
                    var suite = AssemblyLoader.CreateInstance<IPerfTestSuite>(toolAssemblyName, testSuiteTypeName);


                    var researchedAssebmlyName = arguments.ExtractValue("ra");
                    var subjectTypeName = arguments.ExtractValue("st");
                    var subject = AssemblyLoader.CreateInstance(researchedAssebmlyName, subjectTypeName);

                    var testIndex = int.Parse(arguments.ExtractValue("ti"));

                    var start = arguments.ExtractValue("start");
                    var step = arguments.ExtractValue("step");
                    var end = arguments.ExtractValue("end");

                    if (suite != null && subject != null)
                    {
                        var test = suite.Tests[testIndex];
                        var runner = new TestRunner(
                            delegate(int idx) { suite.SetUp(idx, subject); },
                            delegate { test.Test(subject); },
                            delegate { suite.TearDown(subject); },
                            delegate(int idx) { return suite.GetRunDescriptor(idx); },
                            string.IsNullOrEmpty(start) ? 0 : int.Parse(start),
                            string.IsNullOrEmpty(step) ? 1 : int.Parse(step),
                            string.IsNullOrEmpty(end) ? suite.Tests.Length - 1 : int.Parse(end));

                            runner.Subscribe(testObserver);
                    }

                    testObserver.OnCompleted();
                }
                catch (Exception ex)
                {
                    testObserver.OnError(ex);
                }
            }
        }

        private static void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;

            if (ex == null)
            {
                return;
            }

            SendError(ex);
        }

        private static void SendError(Exception ex)
        {
            if (string.IsNullOrEmpty(boxName))
            {
                Console.Error.WriteLine(ex.ToString());
            }
            else
            {
                using (var mailBox = new ProcessMailBox(boxName, 1024))
                {
                    mailBox.Content = new ExperimentError();
                }
            }
        }
    }
}
