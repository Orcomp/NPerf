namespace NPerf.Experiment
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.IO;
    using System.Reflection;
    using NPerf.Framework;
    using System.Windows.Forms;
    using NPerf.Framework.Interfaces;

    internal class Program
    {
        public static void Main(string[] args)
        {
            /* NPerf.Experiment -ta toolAssembly -ft testSuiteTypeName -ti testIndex -ra researchedAssebmly -st subjectType 
             * 
             * toolAssembly - file name of the assemly with test suites
             * testSuiteTypeName - the name of type, which implements IPerfTestSuite interface
             * testIndex - index of test method in executed test suite
             */

            try
            {
                var arguments = args.ConvertToArguments();

                var toolAssemblyName = arguments.ExtractValue("ta");
                var testSuiteTypeName = arguments.ExtractValue("ft");
                var suite = AssemblyLoader.CreateInstance<IPerfTestSuite>(toolAssemblyName, testSuiteTypeName);


                var researchedAssebmlyName = arguments.ExtractValue("ra");
                var subjectTypeName = arguments.ExtractValue("st");
                var subject = AssemblyLoader.CreateInstance(researchedAssebmlyName, subjectTypeName);

                var testIndex = int.Parse(arguments.ExtractValue("ti"));

                if (suite != null && subject != null)
                {
                    var runner = new TestRunner(suite, testIndex, subject);
                    runner.RunTests();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());               
            }

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;

            if (ex != null)
            {
                Console.Error.WriteLine(ex.ToString());
            }
        }
    }
}
