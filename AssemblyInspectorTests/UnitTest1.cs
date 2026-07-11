using System;
using System.IO;
using Xunit;
using AssemblyInspector;

namespace AssemblyInspectorTests
{
    public class InspectorTests
    {
        [Fact]
        public void Main_WithNoArguments_PrintsErrorMessage()
        {
            using (var sw = new StringWriter())
            {
                var originalOut = Console.Out;
                Console.SetOut(sw);
                try
                {
                    Program.Main(new string[0]);
                }
                finally
                {
                    Console.SetOut(originalOut);
                }

                var output = sw.ToString();
                Assert.Contains("Укажите путь к .dll файлу", output);
            }
        }

        [Fact]
        public void Main_WithInvalidPath_PrintsFileNotFoundMessage()
        {
            using (var sw = new StringWriter())
            {
                var originalOut = Console.Out;
                Console.SetOut(sw);
                try
                {
                    Program.Main(new string[] { "nonexistent_file.dll" });
                }
                finally
                {
                    Console.SetOut(originalOut);
                }

                var output = sw.ToString();
                Assert.Contains("Файл не найден", output);
            }
        }

        [Fact]
        public void Main_WithValidDll_ExtractsMetadataCorrectly()
        {
            string dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "task07.dll");
            
            if (!File.Exists(dllPath))
            {
                string alternativePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "task07", "bin", "Debug", "net10.0", "task07.dll"));
                if (File.Exists(alternativePath))
                {
                    dllPath = alternativePath;
                }
            }

            Assert.True(File.Exists(dllPath), $"Тестовая сборка не найдена по пути: {dllPath}");

            using (var sw = new StringWriter())
            {
                var originalOut = Console.Out;
                Console.SetOut(sw);
                try
                {
                    Program.Main(new string[] { dllPath });
                }
                finally
                {
                    Console.SetOut(originalOut);
                }

                var output = sw.ToString();
                Assert.Contains("Анализ сборки: task07", output);
                Assert.Contains("Класс: task07.SampleClass", output);
                Assert.Contains("[DisplayNameAttribute]", output);
                Assert.Contains("[VersionAttribute]", output);
                Assert.Contains("Методы:", output);
		Assert.Contains("Конструкторы:", output);
            }
        }
    }
}
