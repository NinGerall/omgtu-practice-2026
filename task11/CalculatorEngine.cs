using System;
using System.IO;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace task11
{
    public interface ICalculator
    {
        int Add(int a, int b);
        int Minus(int a, int b);
        int Mul(int a, int b);
        int Div(int a, int b);
    }

    public static class CompilerService
    {
        public static ICalculator CompileCalculator(string rawSourceCode)
        {
            string modifiedCode = $@"
                using System;
                using task11;

                namespace DynamicGenerated
                {{
                    {rawSourceCode.Replace("public class Calculator", "public class Calculator : ICalculator")}
                }}";

            var syntaxTree = CSharpSyntaxTree.ParseText(modifiedCode);
            string assemblyName = "DynamicCalculator_" + Guid.NewGuid().ToString("N");

            var references = new MetadataReference[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(ICalculator).Assembly.Location),
                MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Runtime")).Location)
            };

            var compilation = CSharpCompilation.Create(assemblyName)
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(references)
                .AddSyntaxTrees(syntaxTree);

            using (var ms = new MemoryStream())
            {
                var result = compilation.Emit(ms);
                if (!result.Success)
                {
                    throw new InvalidOperationException("Ошибка динамической компиляции кода.");
                }

                ms.Seek(0, SeekOrigin.Begin);
                var assembly = Assembly.Load(ms.ToArray());
                var type = assembly.GetType("DynamicGenerated.Calculator");
                
                return (ICalculator)Activator.CreateInstance(type);
            }
        }
    }
}