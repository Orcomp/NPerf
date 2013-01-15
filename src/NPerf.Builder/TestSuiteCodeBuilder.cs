namespace NPerf.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using NPerf.Framework;
    using System.CodeDom;
    using CodeDomUtilities;
    using Fasterflect;
    using System.Linq.Expressions;
    using System.Diagnostics;
    using System.Reflection;

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

        public string SetUpMethodName { get; set; }

        public string TearDownMethodName { get; set; }

        public int DefaultTestCount { get; set; }

        public string Description { get; set; }

        public string FeatureDescription { get; set; }

        public TestCodeBuider[] Tests { get; set; }


        public CodeCompileUnit BuildCode()
        {
            CodeCompileUnit compileUnit = CodeDomBlocks.CompileUnit();
            
            CodeNamespace testSuites = CodeDomBlocks.Namespace("NPerf.TestSuites");
            testSuites.Imports.Add(new CodeNamespaceImport("System"));

            compileUnit.AddNamespace(testSuites);
           
            var basePerfTestSuite = new CodeTypeReference(typeof(BasePerfTestSuite<>));
            basePerfTestSuite.TypeArguments.Add(this.testedAbstraction);

            var basePerfTest = new CodeTypeReference(typeof(BasePerfTest<>));
            basePerfTest.TypeArguments.Add(this.testedAbstraction);

            var dynamicTestSuiteClass = CodeDomBlocks.Class(MemberAttributes.Public, testSuiteClassName);
            dynamicTestSuiteClass.BaseTypes.Add(basePerfTestSuite);

            var dynamicTestClass = this.CreatePerfTestClass();

            testSuites.AddType(dynamicTestSuiteClass);
            testSuites.AddType(dynamicTestClass);

            CodeMemberField tester = CodeDomBlocks.Field(MemberAttributes.Private, this.testerType, "tester");
            CodeFieldReferenceExpression testerReference = CodeDomExpressions.This.FieldReference(tester.Name);

            CodeMemberField testedObject = CodeDomBlocks.Field(MemberAttributes.Private, this.testedAbstraction, "testedObject");

            CodeFieldReferenceExpression testedObjectReference = CodeDomExpressions.This.FieldReference(testedObject.Name);
            
            dynamicTestSuiteClass.AddMember(tester);
            dynamicTestSuiteClass.AddMember(testedObject);

            CodeConstructor testSuiteConstructor = CodeDomBlocks.Constructor(MemberAttributes.Public);
            dynamicTestSuiteClass.AddMember(testSuiteConstructor);

            testSuiteConstructor.AddStatement(testerReference.Assign(this.testerType.CreateObject()));
            testSuiteConstructor.AddStatement(testedObjectReference.Assign(this.typeToTest.CreateObject()));
            
            testSuiteConstructor.AddStatement(
                GetPropertyReference<IPerfTestSuite>(x => x.DefaultTestCount)
                .Assign(this.DefaultTestCount.Literal()));

            testSuiteConstructor.AddStatement(
                GetPropertyReference<IPerfTestSuite>(x => x.Description)
                .Assign(this.Description.Literal()));

            testSuiteConstructor.AddStatement(
                GetPropertyReference<IPerfTestSuite>(x => x.FeatureDescription)
                .Assign(this.FeatureDescription.Literal()));

            testSuiteConstructor.AddStatement(
                GetPropertyReference<BasePerfTestSuite<object>>(x => x.SetUpMethod)
                .Assign(testedObjectReference.PropertyReference(this.SetUpMethodName)));

            testSuiteConstructor.AddStatement(
                GetPropertyReference<BasePerfTestSuite<object>>(x => x.TearDownMethod)
                .Assign(testedObjectReference.PropertyReference(this.TearDownMethodName)));            

            return compileUnit;
        }

        private CodeTypeDeclaration CreatePerfTestClass()
        {
            var basePerfTest = new CodeTypeReference(typeof(BasePerfTest<>));
            basePerfTest.TypeArguments.Add(this.testedAbstraction);

            var @string = new CodeTypeReference(typeof(string));
            var @Action = new CodeTypeReference(typeof(Action<>));
            @Action.TypeArguments.Add(this.testedAbstraction);

            var dynamicTestClass = CodeDomBlocks.Class(MemberAttributes.Public, testClassName);
            dynamicTestClass.BaseTypes.Add(basePerfTest);

            CodeConstructor testConstructor = CodeDomBlocks.Constructor(MemberAttributes.Public);
            testConstructor.AddParameter(new CodeParameterDeclarationExpression(@string, "name"));
            testConstructor.AddParameter(new CodeParameterDeclarationExpression(@string, "description"));
            testConstructor.AddParameter(new CodeParameterDeclarationExpression(@Action, "testMethod"));            

            CodeConstructor ignoredTestConstructor = CodeDomBlocks.Constructor(MemberAttributes.Public);
            ignoredTestConstructor.AddParameter(new CodeParameterDeclarationExpression(@string, "name"));
            ignoredTestConstructor.AddParameter(new CodeParameterDeclarationExpression(@string, "description"));
            ignoredTestConstructor.AddParameter(new CodeParameterDeclarationExpression(@string, "ignoreMessage"));         

            dynamicTestClass.AddMember(testConstructor);
            dynamicTestClass.AddMember(ignoredTestConstructor);

            // TODO: add Tests initialising
         //   var nameReference = CodeDomExpressions.This.FieldReference(testedObject.Name)

          //  testConstructor.AddStatement()

            return dynamicTestClass;
        }

        private static CodePropertyReferenceExpression GetPropertyReference<T>(Expression<Func<T, object>> propertyExpression)
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

        public override string ToString()
        {
            var code = @"
using System;
using NPerf.Framework;

namespace NPerf.TestSuites
{
    public slass DynamicTestSuite : BasePerfTestSuite<%TestedAbstraction%>
    {
        private %TestedAbstraction% testedObject;

        public DynamicTestSuite()
        {
            this.testedObject = new %TestedType%();

            this.DefaultTestCount = %DefaultTestCount%;
            this.Description = %Description%;
            this.FeatureDescription = %FeatureDescription%;
            this.SetUpMethod = this.testedObject.%SetUpMethodName%;
            this.TearDownMethod = this.testedObject.%TearDownMethodName%;

            this.Tests = new IPerfTest[]
            {
                %Tests%
            };
        }
    }

    internal class DynamicTest : BasePerfTest<%TestedAbstraction%>
    {
        public DynamicTest(string name, string description, string ignoreMessage)
        {
            this.Name = name;
            this.Description = description;
            this.IsIgnore = true;
            this.IgnoreMessage = ignoreMessage;
        }

        public DynamicTest(string name, string description, Action<%TestedAbstraction%> testMethod)
        {
            this.Name = name;
            this.Description = description;
            this.TestMethod = testMethod;
        }
    }
}
"
                .Replace("%TestedAbstraction%", this.TestedAbstraction)
                .Replace("%TestedType%", this.TypeToTest)
                .Replace("%DefaultTestCount%", this.DefaultTestCount.ToString())
                .Replace("%Description%", this.Description)
                .Replace("%FeatureDescription%", this.FeatureDescription)
                .Replace("%SetUpMethodName%", this.SetUpMethodName)
                .Replace("%TearDownMethodName%", this.TearDownMethodName)
                .Replace("%Tests%", string.Join(@",
                ", (from test in Tests
                    select test.ToString())));

            return code;
        }        
    }
}
