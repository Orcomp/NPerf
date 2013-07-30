namespace NPerf.Lab.TestBuilder
{
    using System;
    using System.CodeDom;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;
    using CodeDomUtilities;
    using NPerf.Core;
    using NPerf.Core.Info;
    using Blocks = CodeDomUtilities.CodeDomBlocks;
    using Expressions = CodeDomUtilities.CodeDomExpressions;

    public class TestSuiteCodeBuilder
    {
        public const string TestSuiteClassName = "GeneratedTestSuite";

        private const string TestClassName = "GeneratedTest";

        private readonly CodeTypeReference testedAbstraction;

        private readonly CodeTypeReference testerType;

        private readonly TestSuiteInfo testSuiteInfo;

        private readonly string runDescriptorMethodName;

        private readonly string setUpMethodName;

        private readonly string tearDownMethodName;

        public TestSuiteCodeBuilder(
            TestSuiteInfo testSuiteInfo,
            string runDescriptorMethodName,
            string setUpMethodName,
            string tearDownMethodName)
        {
            this.testSuiteInfo = testSuiteInfo;
            this.runDescriptorMethodName = runDescriptorMethodName;
            this.setUpMethodName = setUpMethodName;
            this.tearDownMethodName = tearDownMethodName;
            this.testerType = new CodeTypeReference(testSuiteInfo.TesterType);
            this.testedAbstraction = new CodeTypeReference(testSuiteInfo.TestedAbstraction);
        }

        public CodeCompileUnit BuildCode()
        {
            var compileUnit = Blocks.CompileUnit();

            var testSuites = Blocks.Namespace("NPerf.TestSuites");
            testSuites.Imports.Add(new CodeNamespaceImport("System"));

            compileUnit.AddNamespace(testSuites);

            var basePerfTestSuite = new CodeTypeReference(typeof(GenericPerfTestSuite<>));
            basePerfTestSuite.TypeArguments.Add(this.testedAbstraction);

            var basePerfTest = new CodeTypeReference(typeof(GenericPerfTest<>));
            basePerfTest.TypeArguments.Add(this.testedAbstraction);

            var dynamicTestSuiteClass = Blocks.Class(MemberAttributes.Public, TestSuiteClassName);
            dynamicTestSuiteClass.BaseTypes.Add(basePerfTestSuite);

            var perfTestType = new CodeTypeReference(typeof(PerfTest));

            var dynamicTestClass = this.CreatePerfTestClass();

            testSuites.AddType(dynamicTestSuiteClass);
            testSuites.AddType(dynamicTestClass);

            var tester = Blocks.Field(MemberAttributes.Private, this.testerType, "tester");
            var testerReference = Expressions.This.FieldReference(tester.Name);

            dynamicTestSuiteClass.AddMember(tester);

            var tests = from test in this.testSuiteInfo.Tests
                        select
                            new CodeObjectCreateExpression(
                            dynamicTestClass.Name,
                            new CodeTypeReference(typeof(Guid)).CreateObject(test.TestId.ToString().Literal()),
                            test.TestMethodName.Literal(),
                            (CodeExpression)testerReference.PropertyReference(test.TestMethodName),
                            test.TestDescription.Literal(),
                            new CodeTypeOfExpression(test.TestedType));

            var testSuiteConstructor = Blocks.Constructor(MemberAttributes.Public)
                .AddStatement(testerReference.Assign(this.testerType.CreateObject()))
                .AddStatement(PropertyReference<PerfTestSuite>(x => x.DefaultTestCount).Assign(this.testSuiteInfo.DefaultTestCount.Literal()))
                .AddStatement(PropertyReference<PerfTestSuite>(x => x.TestSuiteDescription).Assign(this.testSuiteInfo.TestSuiteDescription.Literal()))
                .AddStatement(PropertyReference<PerfTestSuite>(x => x.FeatureDescription).Assign(this.testSuiteInfo.FeatureDescription.Literal()))
                .AddStatement(
                    PropertyReference<PerfTestSuite>(x => x.Tests)
                        .Assign(new CodeArrayCreateExpression(perfTestType, tests.Cast<CodeExpression>().ToArray())));

            if (!string.IsNullOrEmpty(this.runDescriptorMethodName))
            {
                testSuiteConstructor.AddStatement(
                    PropertyReference<GenericPerfTestSuite<object>>(x => x.DescriptorGetter)
                        .Assign(testerReference.PropertyReference(this.runDescriptorMethodName)));
            }

            if (!string.IsNullOrEmpty(this.setUpMethodName))
            {
                testSuiteConstructor.AddStatement(
                    PropertyReference<GenericPerfTestSuite<object>>(x => x.SetUpMethod)
                        .Assign(testerReference.PropertyReference(this.setUpMethodName)));
            }

            if (!string.IsNullOrEmpty(this.tearDownMethodName))
            {
                testSuiteConstructor.AddStatement(
                    PropertyReference<GenericPerfTestSuite<object>>(x => x.TearDownMethod)
                        .Assign(testerReference.PropertyReference(this.tearDownMethodName)));
            }

            dynamicTestSuiteClass.AddMember(testSuiteConstructor);

            return compileUnit;
        }

        private CodeTypeDeclaration CreatePerfTestClass()
        {
            var basePerfTest = new CodeTypeReference(typeof(GenericPerfTest<>));
            basePerfTest.TypeArguments.Add(this.testedAbstraction);

            var @string = new CodeTypeReference(typeof(string));
            var @action = new CodeTypeReference(typeof(Action<>));
            var @guid = new CodeTypeReference(typeof(Guid));
            var @type = new CodeTypeReference(typeof(Type));
            @action.TypeArguments.Add(this.testedAbstraction);

            var dynamicTestClass = Blocks.Class(MemberAttributes.Public, TestClassName);
            dynamicTestClass.BaseTypes.Add(basePerfTest);

            const string name = "name";
            const string description = "description";
            const string testMethod = "testMethod";
            const string testId = "testId";
            const string testedType = "testedType";

            var testConstructor = Blocks.Constructor(MemberAttributes.Public)
                .AddParameter(new CodeParameterDeclarationExpression(@guid, @testId))
                .AddParameter(new CodeParameterDeclarationExpression(@string, @name))
                .AddParameter(new CodeParameterDeclarationExpression(@action, @testMethod))                
                .AddParameter(new CodeParameterDeclarationExpression(@string, @description))
                .AddParameter(new CodeParameterDeclarationExpression(@type, testedType))
                .AddStatement(PropertyReference<TestInfo>(x => x.TestId).Assign(new CodeVariableReferenceExpression(@testId)))  
                .AddStatement(PropertyReference<TestInfo>(x => x.TestMethodName).Assign(new CodeVariableReferenceExpression(@name)))
                .AddStatement(PropertyReference<TestInfo>(x => x.TestDescription).Assign(new CodeVariableReferenceExpression(@description)))
                .AddStatement(PropertyReference<GenericPerfTest<object>>(x => x.TestMethod).Assign(new CodeVariableReferenceExpression(@testMethod)))
                .AddStatement(PropertyReference<TestInfo>(x => x.TestedType).Assign(new CodeVariableReferenceExpression(testedType)));

            dynamicTestClass.AddMember(testConstructor);
            return dynamicTestClass;
        }

        private static CodePropertyReferenceExpression PropertyReference<T>(Expression<Func<T, object>> propertyExpression)
        {
            MemberExpression body;
            var expression = propertyExpression.Body as UnaryExpression;
            if (expression != null)
            {
                var unaryBody = expression;
                body = unaryBody.Operand as MemberExpression;
            }
            else
            {
                body = propertyExpression.Body as MemberExpression;
            }

            Debug.Assert(body != null, "body != null");

            return CodeDomExpressions.This.PropertyReference(body.Member.Name);
        }
    }
}
