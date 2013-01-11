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

    internal class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            /* NPerf.Experiment -ta toolAssembly -ft fixtureTypeNam -ti testIndex -ra researchedAssebmly -st subjectType 
             * 
             * toolAssembly - file name of the assemly with fixtures
             * fixtureTypeName - the name of type, which implements IFixture<> interface
             * testIndex - index of test method in executed fixture
             */

            try
            {
                var arguments = args.ConvertToArguments();

                var toolAssemblyName = arguments.ExtractValue("ta");
                var fixtureTypeName = arguments.ExtractValue("ft");
                var fixture = AssemblyLoader.CreateInstance<IPerfFixture>(toolAssemblyName, fixtureTypeName);


                var researchedAssebmlyName = arguments.ExtractValue("ra");
                var subjectTypeName = arguments.ExtractValue("st");
                var subject = AssemblyLoader.CreateInstance(researchedAssebmlyName, subjectTypeName);

                if (fixture != null && subject != null)
                {
                   // fixture.
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
