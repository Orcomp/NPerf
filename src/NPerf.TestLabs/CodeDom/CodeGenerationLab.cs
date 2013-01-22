using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.CodeDom;
using CodeDomUtilities;
using NPerf.Framework;
using CodeDomUtilities.Patterns;
using System.Reflection;
using NPerf.DevHelpers;
using NPerf.Lab.TestBuilder;
using NPerf.Framework.Interfaces;

namespace NPerf.TestLabs.CodeDom
{
    using NPerf.Core;

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

            this.Trace(builder.BuildCode().GetVB());
        }


        public void CodeFromTypes()
        {
            var builder = new TestSuiteAssemblyBuilder(typeof(AttribitedFixtureSample), typeof(TestedObject));
            this.TraceLine(builder.CreateSourceCode());
        }

        public void AssemblyFromTypes()
        {
            try
            {
                var builder = new TestSuiteAssemblyBuilder(typeof(AttribitedFixtureSample), typeof(TestedObject));
                builder.CreateTestSuite();
            }
            catch (Exception ex)
            {
                this.TraceError(ex);
            }
        }

        public void GenerateCode()
        {
            var compileUnit = new CodeCompileUnit();
            var testSuites = new CodeNamespace("NPerf.TestSuites");
            testSuites.Imports.Add(new CodeNamespaceImport("System"));

            compileUnit.AddNamespace(testSuites);

            var dynamicTestSuite = new CodeTypeDeclaration("DynamicTestSuite") { IsClass = true };
            var basePerfTestSuite = new CodeTypeReference(typeof(AbstractPerfTestSuite<>));
            basePerfTestSuite.TypeArguments.Add("ITestedAbstraction");

            dynamicTestSuite.BaseTypes.Add(basePerfTestSuite);
            
            var tester =
                new CodeMemberField 
                { 
                    Name = "tester",  
                    Attributes = MemberAttributes.Private, 
                    Type = new CodeTypeReference("TesterType")
                };


            var testerReference =
                new CodeFieldReferenceExpression(
                    new CodeThisReferenceExpression(), "tester");

            var testerInit = testerReference.Assign(new CodeTypeReference("TesterType").CreateObject());

            var constructor =
                new CodeConstructor { Attributes = MemberAttributes.Public | MemberAttributes.Final };
            constructor.AddStatement(testerInit);

            dynamicTestSuite.AddMember(tester);
            dynamicTestSuite.AddMember(constructor);
            

            testSuites.AddType(dynamicTestSuite);

            this.Trace(compileUnit.GetVB());
        }
        
    }
}