using System;
using System.Reflection;
using System.Text;

namespace task07
{
    public static class ReflectionHelper
    {
        public static string PrintTypeInfo(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            var sb = new StringBuilder();

            var classDisplay = type.GetCustomAttribute<DisplayNameAttribute>();
            var classVersion = type.GetCustomAttribute<VersionAttribute>();

            sb.AppendLine($"Класс: {type.Name}");
            if (classDisplay != null) sb.AppendLine($"Понятное имя класса: {classDisplay.DisplayName}");
            if (classVersion != null) sb.AppendLine($"Версия класса: {classVersion.Major}.{classVersion.Minor}");

            sb.AppendLine();
            sb.AppendLine("Свойства:");
            foreach (var prop in type.GetProperties())
            {
                var propDisplay = prop.GetCustomAttribute<DisplayNameAttribute>();
                sb.Append($"- {prop.Name} (тип: {prop.PropertyType.Name})");
                if (propDisplay != null) sb.Append($" [Понятное имя: {propDisplay.DisplayName}]");
                sb.AppendLine();
            }

            sb.AppendLine();
            sb.AppendLine("Методы:");
            foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                if (method.IsSpecialName) continue;

                var methodDisplay = method.GetCustomAttribute<DisplayNameAttribute>();
                sb.Append($"- {method.Name}");
                if (methodDisplay != null) sb.Append($" [Понятное имя: {methodDisplay.DisplayName}]");
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}