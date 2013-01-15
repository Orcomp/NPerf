using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CSharp;
using System.CodeDom.Compiler;

namespace NPerf.Builder
{
    internal class CodeCompiler
    {
        public void CompileCode(string sourceCode, string fileName)
        {
            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters parameters = new CompilerParameters();

            parameters.GenerateExecutable = false;
            parameters.OutputAssembly = fileName;
            parameters.GenerateInMemory = false;

           // provider.
        }
    }
}
