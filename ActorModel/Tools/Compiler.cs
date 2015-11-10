using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorModel.Tools
{
    internal class Compiler
    {
        CSharpCodeProvider codeProvider;
        CompilerParameters parameters;

        public Compiler()
        {
            codeProvider = new CSharpCodeProvider();
            parameters = new CompilerParameters(new [] { "mscorlib.dll", "System.Core.dll" });
        }

        public void AddAssembly(string assembly)
        {
            parameters.ReferencedAssemblies.Add(assembly);
        }

        public void Compile(string output, string fileName)
        {
            parameters.GenerateExecutable = false;
            parameters.OutputAssembly = output;
            CompilerResults results = codeProvider.CompileAssemblyFromFile(parameters, fileName);
            
            var errors = results.Errors.Cast<CompilerError>().ToList();
            if (errors.Count > 0)
                throw new FormatException("Compilation error in file " + fileName);
        }
    }
}
