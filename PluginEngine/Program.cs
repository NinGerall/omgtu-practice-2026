using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using PluginsBase;

namespace PluginEngine
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string pluginsDir = args.Length > 0 ? args[0] : AppDomain.CurrentDomain.BaseDirectory;
            ExecutePlugins(pluginsDir);
        }

        public static void ExecutePlugins(string targetDir)
        {
            if (!Directory.Exists(targetDir)) return;

            var pluginTypes = new List<(Type Type, PluginLoadAttribute Attr)>();

            foreach (var file in Directory.GetFiles(targetDir, "*.dll"))
            {
                try
                {
                    var assembly = Assembly.LoadFrom(file);
                    foreach (var type in assembly.GetTypes())
                    {
                        if (typeof(IPlugin).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                        {
                            var attr = type.GetCustomAttribute<PluginLoadAttribute>();
                            if (attr != null)
                            {
                                pluginTypes.Add((type, attr));
                            }
                        }
                    }
                }
                catch { }
            }

            var sortedTypes = TopologicalSort(pluginTypes);

            foreach (var type in sortedTypes)
            {
                try
                {
                    var plugin = (IPlugin)Activator.CreateInstance(type);
                    plugin.Execute();
                }
                catch { }
            }
        }

        public static List<Type> TopologicalSort(List<(Type Type, PluginLoadAttribute Attr)> plugins)
        {
            var result = new List<Type>();
            var visited = new Dictionary<string, bool>(); 
            var pluginMap = plugins.ToDictionary(p => p.Attr.Name, p => p.Type);
            var attrMap = plugins.ToDictionary(p => p.Attr.Name, p => p.Attr);

            void Visit(string name)
            {
                if (!pluginMap.ContainsKey(name)) return;
                if (visited.TryGetValue(name, out bool inProcess))
                {
                    if (inProcess) throw new InvalidOperationException("Обнаружена циклическая зависимость плагинов!");
                    return;
                }

                visited[name] = true; 

                foreach (var dep in attrMap[name].Dependencies)
                {
                    Visit(dep);
                }

                visited[name] = false; 
                if (!result.Contains(pluginMap[name]))
                {
                    result.Add(pluginMap[name]);
                }
            }

            foreach (var plugin in plugins)
            {
                if (!visited.ContainsKey(plugin.Attr.Name))
                {
                    Visit(plugin.Attr.Name);
                }
            }

            return result;
        }
    }
}
