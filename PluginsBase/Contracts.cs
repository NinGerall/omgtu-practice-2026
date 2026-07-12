using System;

namespace PluginsBase
{
    public interface IPlugin
    {
        void Execute();
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class PluginLoadAttribute : Attribute
    {
        public string Name { get; }
        public string[] Dependencies { get; }

        public PluginLoadAttribute(string name, params string[] dependencies)
        {
            Name = name;
            Dependencies = dependencies ?? Array.Empty<string>();
        }
    }
}