using System;
using System.IO;
using System.Reflection;
using CommandLib;

namespace CommandRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            string dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FileSystemCommands.dll");

            if (!File.Exists(dllPath))
            {
                string alternativePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "FileSystemCommands", "bin", "Debug", "net10.0", "FileSystemCommands.dll"));
                if (File.Exists(alternativePath))
                {
                    dllPath = alternativePath;
                }
            }

            if (!File.Exists(dllPath))
            {
                Console.WriteLine("Файл FileSystemCommands.dll не найден.");
                return;
            }

            try
            {
                Assembly assembly = Assembly.LoadFrom(dllPath);

                Type sizeCommandType = assembly.GetType("FileSystemCommands.DirectorySizeCommand");
                if (sizeCommandType != null)
                {
                    string currentDir = AppDomain.CurrentDomain.BaseDirectory;
                    object sizeCommandInstance = Activator.CreateInstance(sizeCommandType, new object[] { currentDir });
                    if (sizeCommandInstance is ICommand cmd)
                    {
                        cmd.Execute();
                    }
                }

                Type findCommandType = assembly.GetType("FileSystemCommands.FindFilesCommand");
                if (findCommandType != null)
                {
                    string currentDir = AppDomain.CurrentDomain.BaseDirectory;
                    object findCommandInstance = Activator.CreateInstance(findCommandType, new object[] { currentDir, "*.json" });
                    if (findCommandInstance is ICommand cmd)
                    {
                        cmd.Execute();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при динамической загрузке: {ex.Message}");
            }
        }
    }
}