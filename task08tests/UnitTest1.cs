using System;
using System.IO;
using Xunit;
using FileSystemCommands;

namespace task08tests
{
    public class FileSystemCommandsTests
    {
        [Fact]
        public void DirectorySizeCommand_ShouldCalculateSize()
        {
            var testDir = Path.Combine(Path.GetTempPath(), "TestDir_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(testDir);
            File.WriteAllText(Path.Combine(testDir, "test1.txt"), "Hello"); // 5 байт
            File.WriteAllText(Path.Combine(testDir, "test2.txt"), "World!"); // 6 байт

            var command = new DirectorySizeCommand(testDir);
            
            using (var sw = new StringWriter())
            {
                var originalOut = Console.Out;
                Console.SetOut(sw);
                try
                {
                    command.Execute();
                }
                finally
                {
                    Console.SetOut(originalOut);
                }

                var output = sw.ToString();
                Assert.Contains("Размер каталога", output);
                Assert.Contains("11 байт", output);
            }

            Directory.Delete(testDir, true);
        }

        [Fact]
        public void FindFilesCommand_ShouldFindMatchingFiles()
        {
            var testDir = Path.Combine(Path.GetTempPath(), "TestDir_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(testDir);
            File.WriteAllText(Path.Combine(testDir, "file1.txt"), "Text");
            File.WriteAllText(Path.Combine(testDir, "file2.log"), "Log");

            var command = new FindFilesCommand(testDir, "*.txt");
            
            using (var sw = new StringWriter())
            {
                var originalOut = Console.Out;
                Console.SetOut(sw);
                try
                {
                    command.Execute();
                }
                finally
                {
                    Console.SetOut(originalOut);
                }

                var output = sw.ToString();
                Assert.Contains("Найдено файлов по маске", output);
                Assert.Contains("file1.txt", output);
                Assert.DoesNotContain("file2.log", output);
            }

            Directory.Delete(testDir, true);
        }
    }
}
