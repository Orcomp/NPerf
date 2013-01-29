namespace NPerf.Lab.TestBuilder
{
    using System;
    using System.CodeDom;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;
    using CodeDomUtilities;
    using NPerf.Core;
    using NPerf.Framework.Interfaces;
    using Blocks = CodeDomUtilities.CodeDomBlocks;
    using Expressions = CodeDomUtilities.CodeDomExpressions;

    public class TestSuiteCodeBuilder : IPerfTestSuiteInfo
    {
        public const string TestSuiteClassName = "GeneratedTestSuite";
        private const string TestClassName = "GeneratedTest";

        private CodeTypeReference testedAbstraction;
        private CodeTypeReference testerType;
        private CodeTypeReference typeToTest;

        public string TesterType
        {
            get { return this.testerType.BaseType; }
            set { this.testerType = new CodeTypeReference(value); }
        }

        public string TestedAbstraction
        {
            get { return this.testedAbstraction.BaseType; }
            set { this.testedAbstraction = new CodeTypeReference(value); }
        }

        public string TypeToTest
        {
            get { return this.typeToTest.BaseType; }
            set { this.typeToTest = new CodeTypeReference(value); }
        }

        public string RunDescriptorMethodName { get; set; }

        public string SetUpMethodName { get; set; }

        public string TearDownMethodName { get; set; }

        public int DefaultTestCount { get; set; }

        public string Description { get; set; }

        public string FeatureDescription { get; set; }

        public TestInfo[] Tests { get; set; }


        public CodeCompileUnit BuildCode()
        {
            var compileUnit = Blocks.CompileUnit();

            var testSuites = Blocks.Namespace("NPerf.TestSuites");
            testSuites.Imports.Add(new CodeNamespaceImport("System"));

            compileUnit.AddNamespace(testSuites);

            var basePerfTestSuite = new CodeTypeReference(typeof(AbstractPerfTestSuite<>));
            basePerfTestSuite.TypeArguments.Add(this.testedAbstraction);

            var basePerfTest = new CodeTypeReference(typeof(AbstractPerfTest<>));
            basePerfTest.TypeArguments.Add(this.testedAbstraction);

            var dynamicTestSuiteClass = Blocks.Class(MemberAttributes.Public, TestSuiteClassName);
            dynamicTestSuiteClass.BaseTypes.Add(basePerfTestSuite);

            var perfTestType = new CodeTypeReference(typeof(IPerfTest));

            var dynamicTestClass = this.CreatePerfTestClass();

            testSuites.AddType(dynamicTestSuiteClass);
            testSuites.AddType(dynamicTestClass);

            var tester = Blocks.Field(MemberAttributes.Private, this.testerType, "tester");
            CodeFieldReferenceExpression testerReference = Expressions.This.FieldReference(tester.Name);

            var testedObject = Blocks.Field(MemberAttributes.Private, this.testedAbstraction, "testedObject");

            CodeFieldReferenceExpression testedObjectReference = Expressions.This.FieldReference(testedObject.Name);

            dynamicTestSuiteClass.AddMember(tester);
            dynamicTestSuiteClass.AddMember(testedObject);

            CodeConstructor testSuiteConstructor = Blocks.Constructor(MemberAttributes.Public)
                .AddStatement(testerReference.Assign(this.testerType.CreateObject()))
                .AddStatement(testedObjectReference.Assign(this.typeToTest.CreateObject()))
                .AddStatement(PropertyReference<IPerfTestSuite>(x => x.DefaultTestCount).Assign(this.DefaultTestCount.Literal()))
                .AddStatement(PropertyReference<IPerfTestSuite>(x => x.Description).Assign(this.Description.Literal()))
                .AddStatement(PropertyReference<IPerfTestSuite>(x => x.FeatureDescription).Assign(this.FeatureDescription.Literal()))
                .AddStatement(PropertyReference<AbstractPerfTestSuite<object>>(x => x.SetUpMethod).Assign(testerReference.PropertyReference(this.SetUpMethodName)))
                .AddStatement(PropertyReference<AbstractPerfTestSuite<object>>(x => x.TearDownMethod).Assign(testerReference.PropertyReference(this.TearDownMethodName)))
                .AddStatement(PropertyReference<AbstractPerfTestSuite<object>>(x => x.GetDescriptorMethod).Assign(testerReference.PropertyReference(this.RunDescriptorMethodName)))
                .AddStatement(PropertyReference<IPerfTestSuite>(x => x.Tests).Assign(new CodeArrayCreateExpression(perfTestType,
                    (from test in this.Tests
                     select new CodeObjectCreateExpression(
                         dynamicTestClass.Name,
                         test.Name.Literal(),
                         test.Description.Literal(),
                         (CodeExpression)testerReference.PropertyReference(test.Name))).ToArray())));

            dynamicTestSuiteClass.AddMember(testSuiteConstructor);

            return compileUnit;
        }

        private CodeTypeDeclaration CreatePerfTestClass()
        {
            var basePerfTest = new CodeTypeReference(typeof(AbstractPerfTest<>));
            basePerfTest.TypeArguments.Add(this.testedAbstraction);

            var @string = new CodeTypeReference(typeof(string));
            var @action = new CodeTypeReference(typeof(Action<>));
            @action.TypeArguments.Add(this.testedAbstraction);

            var dynamicTestClass = Blocks.Class(MemberAttributes.Public, TestClassName);
            dynamicTestClass.BaseTypes.Add(basePerfTest);

            const string name = "name";
            const string description = "description";
            const string testMethod = "testMethod";
            const string testMethodName = "testMethodName";

            var testConstructor = Blocks.Constructor(MemberAttributes.Public)
                .AddParameter(new CodeParameterDeclarationExpression(@string, @name))
                .AddParameter(new CodeParameterDeclarationExpression(@string, @description))
                .AddParameter(new CodeParameterDeclarationExpression(@string, @testMethodName))
                .AddParameter(new CodeParameterDeclarationExpression(@action, @testMethod))
                .AddStatement(PropertyReference<IPerfTestInfo>(x => x.Name).Assign(new CodeVariableReferenceExpression(@name)))
                .AddStatement(PropertyReference<IPerfTestInfo>(x => x.Name).Assign(new CodeVariableReferenceExpression(@testMethodName)))
                .AddStatement(PropertyReference<IPerfTestInfo>(x => x.Description).Assign(new CodeVariableReferenceExpression(@description)))
                .AddStatement(PropertyReference<AbstractPerfTest<object>>(x => x.TestMethod).Assign(new CodeVariableReferenceExpression(@testMethod)));

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

        public object BasePerfTestSuite { get; set; }
    }
}
