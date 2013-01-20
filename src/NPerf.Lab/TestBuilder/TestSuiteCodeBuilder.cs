namespace NPerf.Lab.TestBuilder
{
    using System;
    using System.CodeDom;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;
    using CodeDomUtilities;

    using NPerf.Experiment.Basics;
    using NPerf.Framework;
    using Blocks = CodeDomUtilities.CodeDomBlocks;
    using Expressions = CodeDomUtilities.CodeDomExpressions;
    using NPerf.Framework.Interfaces;
 //   using NPerf.Experiment.Basics;

    public class TestSuiteCodeBuilder : IPerfTestSuiteInfo
    {
        private const string testSuiteClassName = "GeneratedTestSuite";
        private const string testClassName = "GeneratedTest";

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
           
            var basePerfTestSuite = new CodeTypeReference(typeof(BasePerfTestSuite<>));
            basePerfTestSuite.TypeArguments.Add(this.testedAbstraction);

            var basePerfTest = new CodeTypeReference(typeof(BasePerfTest<>));
            basePerfTest.TypeArguments.Add(this.testedAbstraction);

            var dynamicTestSuiteClass = Blocks.Class(MemberAttributes.Public, testSuiteClassName);
            dynamicTestSuiteClass.BaseTypes.Add(basePerfTestSuite);

            var perfTestType = new CodeTypeReference(typeof(IPerfTest));

            var dynamicTestClass = this.CreatePerfTestClass();

            testSuites.AddType(dynamicTestSuiteClass);
            testSuites.AddType(dynamicTestClass);

            CodeMemberField tester = Blocks.Field(MemberAttributes.Private, this.testerType, "tester");
            CodeFieldReferenceExpression testerReference = Expressions.This.FieldReference(tester.Name);

            CodeMemberField testedObject = Blocks.Field(MemberAttributes.Private, this.testedAbstraction, "testedObject");

            CodeFieldReferenceExpression testedObjectReference = Expressions.This.FieldReference(testedObject.Name);
            
            dynamicTestSuiteClass.AddMember(tester);
            dynamicTestSuiteClass.AddMember(testedObject);

            CodeConstructor testSuiteConstructor = Blocks.Constructor(MemberAttributes.Public)
                .AddStatement(testerReference.Assign(this.testerType.CreateObject()))
                .AddStatement(testedObjectReference.Assign(this.typeToTest.CreateObject()))
                .AddStatement(PropertyReference<IPerfTestSuite>(x => x.DefaultTestCount).Assign(this.DefaultTestCount.Literal()))
                .AddStatement(PropertyReference<IPerfTestSuite>(x => x.Description).Assign(this.Description.Literal()))
                .AddStatement(PropertyReference<IPerfTestSuite>(x => x.FeatureDescription).Assign(this.FeatureDescription.Literal()))
                .AddStatement(PropertyReference<BasePerfTestSuite<object>>(x => x.SetUpMethod).Assign(testerReference.PropertyReference(this.SetUpMethodName)))
                .AddStatement(PropertyReference<BasePerfTestSuite<object>>(x => x.TearDownMethod).Assign(testerReference.PropertyReference(this.TearDownMethodName)))
                .AddStatement(PropertyReference<BasePerfTestSuite<object>>(x => x.GetDescriptorMethod).Assign(testerReference.PropertyReference(this.RunDescriptorMethodName)))
                .AddStatement(PropertyReference<IPerfTestSuite>(x => x.Tests).Assign(new CodeArrayCreateExpression(perfTestType,
                    (from test in this.Tests
                     select new CodeObjectCreateExpression(
                         dynamicTestClass.Name,
                         test.Name.Literal(),
                         test.Description.Literal(),
                         test.IsIgnore
                            ? (CodeExpression)test.IgnoreMessage.Literal()
                            : (CodeExpression)testerReference.PropertyReference(test.Name))).ToArray())));

            dynamicTestSuiteClass.AddMember(testSuiteConstructor);

            return compileUnit;
        }

        private CodeTypeDeclaration CreatePerfTestClass()
        {
            var basePerfTest = new CodeTypeReference(typeof(BasePerfTest<>));
            basePerfTest.TypeArguments.Add(this.testedAbstraction);

            var @string = new CodeTypeReference(typeof(string));
            var @action = new CodeTypeReference(typeof(Action<>));
            @action.TypeArguments.Add(this.testedAbstraction);

            var dynamicTestClass = Blocks.Class(MemberAttributes.Public, testClassName);
            dynamicTestClass.BaseTypes.Add(basePerfTest);

            const string name = "name";
            const string description = "description";
            const string testMethod = "testMethod";
            const string ignoreMessage = "ignoreMessage";

            CodeConstructor testConstructor = Blocks.Constructor(MemberAttributes.Public)
                .AddParameter(new CodeParameterDeclarationExpression(@string, @name))
                .AddParameter(new CodeParameterDeclarationExpression(@string, @description))
                .AddParameter(new CodeParameterDeclarationExpression(@action, @testMethod))
                .AddStatement(PropertyReference<IPerfTestInfo>(x => x.Name).Assign(new CodeVariableReferenceExpression(@name)))
                .AddStatement(PropertyReference<IPerfTestInfo>(x => x.Description).Assign(new CodeVariableReferenceExpression(@description)))
                .AddStatement(PropertyReference<BasePerfTest<object>>(x => x.TestMethod).Assign(new CodeVariableReferenceExpression(@testMethod)));            

            CodeConstructor ignoredTestConstructor = Blocks.Constructor(MemberAttributes.Public)
                .AddParameter(new CodeParameterDeclarationExpression(@string, @name))
                .AddParameter(new CodeParameterDeclarationExpression(@string, @description))
                .AddParameter(new CodeParameterDeclarationExpression(@string, @ignoreMessage))
                .AddStatement(PropertyReference<IPerfTestInfo>(x => x.Name).Assign(new CodeVariableReferenceExpression(@name)))
                .AddStatement(PropertyReference<IPerfTestInfo>(x => x.Description).Assign(new CodeVariableReferenceExpression(@description)))
                .AddStatement(PropertyReference<BasePerfTest<object>>(x => x.IsIgnore).Assign((true).Literal()))
                .AddStatement(PropertyReference<BasePerfTest<object>>(x => x.IgnoreMessage).Assign(new CodeVariableReferenceExpression("ignoreMessage")));            

            dynamicTestClass.AddMember(testConstructor);
            dynamicTestClass.AddMember(ignoredTestConstructor);                        

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
