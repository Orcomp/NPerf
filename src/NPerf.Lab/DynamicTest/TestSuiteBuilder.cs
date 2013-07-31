namespace NPerf.Lab.TestBuilder
{
    using System;
    using System.CodeDom.Compiler;
    using System.Linq;
    using System.Reflection;
    using CodeDomUtilities;
    using Fasterflect;
    using Microsoft.CSharp;
    using NPerf.Core;
    using NPerf.Core.Info;
    using NPerf.Framework;

    public class TestSuiteBuilder
    {
        private readonly TestSuiteInfo testSuiteInfo;

        private readonly MethodInfo runDescriptor;

        private readonly MethodInfo setUp;

        private readonly MethodInfo tearDown;


        public TestSuiteBuilder(TestSuiteInfo testSuiteInfo)
        {
            if (testSuiteInfo == null)
            {
                throw new ArgumentNullException("testSuiteInfo");
            }

            this.testSuiteInfo = testSuiteInfo;

            // get run descriptor
            this.runDescriptor =
                testSuiteInfo.TesterType.MethodsWith(Flags.AllMembers, typeof(PerfRunDescriptorAttribute))
                             .FirstOrDefault(
                                 m => m.ReturnType == typeof(double) && m.HasParameterSignature(new[] { typeof(int) }));

            // get set up
            this.setUp =
                testSuiteInfo.TesterType.MethodsWith(Flags.AllMembers, typeof(PerfSetUpAttribute))
                             .FirstOrDefault(
                                 m =>
                                 m.ReturnType == typeof(void)
                                 && m.HasParameterSignature(new[] { typeof(int), testSuiteInfo.TestedAbstraction }));

            // get tear down
            this.tearDown =
                testSuiteInfo.TesterType.MethodsWith(Flags.AllMembers, typeof(PerfTearDownAttribute))
                             .FirstOrDefault(
                                 m =>
                                 m.ReturnType == typeof(void)
                                 && m.HasParameterSignature(new[] { testSuiteInfo.TestedAbstraction }));
        }

        private string CreateSourceCode()
        {
            var testSuiteCode = new TestSuiteCodeBuilder(
                this.testSuiteInfo,
                this.runDescriptor == null ? string.Empty : this.runDescriptor.Name,
                this.setUp == null ? string.Empty : this.setUp.Name,
                this.tearDown == null ? string.Empty : this.tearDown.Name);

            return testSuiteCode.BuildCode().GetCSharp();
        }
        
        public string Build()
        {
            var codeProvider = new CSharpCodeProvider();
            var parameters = new CompilerParameters();

            parameters.ReferencedAssemblies.Add("System.dll");
            var assemblies = (new[]
                {
                    this.testSuiteInfo.TestedAbstraction.Assembly,
                    this.testSuiteInfo.TesterType.Assembly, 
                    typeof (PerfTest).Assembly,
                    typeof (PerfTesterAttribute).Assembly,
                    typeof (GenericPerfTest<>).Assembly
                }).Union(this.testSuiteInfo.Tests.Select(x => x.TestedType.Assembly))
                  .Distinct();
            foreach (var assm in assemblies)
            {
                parameters.ReferencedAssemblies.Add(assm.Location);
            }
            
            parameters.GenerateInMemory = false;
            parameters.IncludeDebugInformation = true;

            var code = this.CreateSourceCode();
            var results = codeProvider.CompileAssemblyFromSource(parameters, code);

            if (results.Errors.HasErrors)
            {
                var errorMessage = results.Errors.Count + " Errors:";

                for (var x = 0; x < results.Errors.Count; x++)
                {
                    errorMessage = errorMessage + "\r\nLine: " + results.Errors[x].Line + " - " + results.Errors[x].ErrorText;
                }

                throw new Exception(errorMessage);
            }




            return results.PathToAssembly;
        }
    }
}
