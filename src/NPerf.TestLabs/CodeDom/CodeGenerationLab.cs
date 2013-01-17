using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.CodeDom;
using CodeDomUtilities;
using NPerf.Framework;
using CodeDomUtilities.Patterns;
using System.Reflection;
using NPerf.Builder;
using NPerf.DevHelpers;

namespace NPerf.TestLabs.CodeDom
{
    [DisplayName("")]
    [Description("")]
    public sealed class CodeGenerationLab : BaseConsoleLab
    {
        protected override void Main()
        {
            TraceLine("(Optional) Print instructions for users here.");
            TraceLine();

            // TODO: Implementation goes here.
        }


        public void UseBuilder()
        {
            var builder = new TestSuiteCodeBuilder 
            {
                Description = "Description",
                DefaultTestCount = 10,
                FeatureDescription = "FeatureDescription",
                SetUpMethodName = "SetUpMethodName",
                TearDownMethodName = "TearDownMethodName",
                TestedAbstraction = "TestedAbstraction",
                TesterType = "TesterType",
                TypeToTest = "TypeToTest",
                Tests = new[]
                {
                    new TestInfo{ Name = "Test1", Description = "TestDescr"},
                    new TestInfo{ Name = "Test2", Description = "TestDescr2", IsIgnore = true},
                    new TestInfo{ Name = "Test3", Description = "TestDescr3", IsIgnore = true, IgnoreMessage="Ignore message"}
                }
            };

            Trace(builder.BuildCode().GetVB());
        }


        public void CodeFromTypes()
        {
            var builder = new TestSuiteAssemblyBuilder(typeof(AttribitedFixtureSample), typeof(TestedObject));
            TraceLine(builder.CreateSourceCode());
        }

        public void AssemblyFromTypes()
        {
            try
            {
                var builder = new TestSuiteAssemblyBuilder(typeof(AttribitedFixtureSample), typeof(TestedObject));
                var assm = builder.CreateTestSuite();
            }
            catch (Exception ex)
            {
                TraceError(ex);
            }
        }

        public void GenerateCode()
        {
            CodeCompileUnit compileUnit = new CodeCompileUnit();
            CodeNamespace testSuites = new CodeNamespace("NPerf.TestSuites");
            testSuites.Imports.Add(new CodeNamespaceImport("System"));

            compileUnit.AddNamespace(testSuites);

            CodeTypeDeclaration dynamicTestSuite = new CodeTypeDeclaration("DynamicTestSuite");
            dynamicTestSuite.IsClass = true;
            var basePerfTestSuite = new CodeTypeReference(typeof(BasePerfTestSuite<>));
            basePerfTestSuite.TypeArguments.Add("ITestedAbstraction");

            dynamicTestSuite.BaseTypes.Add(basePerfTestSuite);

            
            CodeMemberField tester =
                new CodeMemberField 
                { 
                    Name = "tester",  
                    Attributes = MemberAttributes.Private, 
                    Type = new CodeTypeReference("TesterType")
                };


            CodeFieldReferenceExpression testerReference =
                new CodeFieldReferenceExpression(
                    new CodeThisReferenceExpression(), "tester");

            var testerInit = testerReference.Assign(new CodeTypeReference("TesterType").CreateObject());

            /*
            var tester = "TesterType".PropertyReference("tester");
            var assignment = tester.Assign((null as object).Literal());
            */
            CodeConstructor constructor =
                new CodeConstructor { Attributes = MemberAttributes.Public | MemberAttributes.Final };
            constructor.AddStatement(testerInit);

            dynamicTestSuite.AddMember(tester);
            dynamicTestSuite.AddMember(constructor);
            

            testSuites.AddType(dynamicTestSuite);

            Trace(compileUnit.GetVB());
        }
        
    }
}