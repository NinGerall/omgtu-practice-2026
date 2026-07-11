using System;
using System.IO;
using System.Reflection;

namespace AssemblyInspector
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Укажите путь к .dll файлу в параметрах командной строки.");
                return;
            }

            string dllPath = Path.GetFullPath(args[0]);
            if (!File.Exists(dllPath))
            {
                Console.WriteLine($"Файл не найден: {dllPath}");
                return;
            }

            try
            {
                Assembly assembly = Assembly.LoadFrom(dllPath);
                Console.WriteLine($"Анализ сборки: {assembly.GetName().Name}");
                Console.WriteLine(new string('=', 40));

                Type[] types = assembly.GetTypes();
                foreach (Type type in types)
                {
                    if (!type.IsClass) continue;

                    Console.WriteLine($"\nКласс: {type.FullName}");
                    
                    var classAttrs = type.GetCustomAttributes(false);
                    if (classAttrs.Length > 0)
                    {
                        Console.WriteLine("  Атрибуты класса:");
                        foreach (var attr in classAttrs)
                        {
                            Console.WriteLine($"    [{attr.GetType().Name}]");
                        }
                    }

                    ConstructorInfo[] constructors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic);
                    if (constructors.Length > 0)
                    {
                        Console.WriteLine("  Конструкторы:");
                        foreach (var ctor in constructors)
                        {
                            Console.Write($"    {type.Name}(");
                            ParameterInfo[] parameters = ctor.GetParameters();
                            for (int i = 0; i < parameters.Length; i++)
                            {
                                Console.Write($"{parameters[i].ParameterType.Name} {parameters[i].Name}");
                                if (i < parameters.Length - 1) Console.Write(", ");
                            }
                            Console.WriteLine(")");
                        }
                    }

                    MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly);
                    if (methods.Length > 0)
                    {
                        Console.WriteLine("  Методы:");
                        foreach (MethodInfo method in methods)
                        {
                            if (method.IsSpecialName) continue;

                            var methodAttrs = method.GetCustomAttributes(false);
                            if (methodAttrs.Length > 0)
                            {
                                foreach (var attr in methodAttrs)
                                {
                                    Console.WriteLine($"    [{attr.GetType().Name}]");
                                }
                            }

                            Console.Write($"    {method.ReturnType.Name} {method.Name}(");
                            ParameterInfo[] parameters = method.GetParameters();
                            for (int i = 0; i < parameters.Length; i++)
                            {
                                Console.Write($"{parameters[i].ParameterType.Name} {parameters[i].Name}");
                                if (i < parameters.Length - 1) Console.Write(", ");
                            }
                            Console.WriteLine(")");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка анализа сборки: {ex.Message}");
            }
        }
    }
}
