using System;
using System.IO;
using CommandLib;

namespace FileSystemCommands
{
    public class DirectorySizeCommand : ICommand
    {
        private readonly string _path;

        public DirectorySizeCommand(string path)
        {
            _path = path;
        }

        public void Execute()
        {
            if (!Directory.Exists(_path))
            {
                Console.WriteLine($"Каталог {_path} не найден.");
                return;
            }

            long size = GetDirectorySize(_path);
            Console.WriteLine($"Размер каталога '{_path}': {size} байт.");
        }

        private long GetDirectorySize(string path)
        {
            long size = 0;
            try
            {
                var dir = new DirectoryInfo(path);
                foreach (var file in dir.GetFiles())
                {
                    size += file.Length;
                }
                foreach (var subDir in dir.GetDirectories())
                {
                    size += GetDirectorySize(subDir.FullName);
                }
            }
            catch
            {
            }
            return size;
        }
    }
}