using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace task05
{
    public class ClassAnalyzer
    {
        private readonly Type _type;

        public ClassAnalyzer(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            _type = type;
        }

        public IEnumerable<string> GetPublicMethods() => _type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static).Where(m => !m.IsSpecialName).Select(m => m.Name);

        public IEnumerable<string> GetMethodParams(string methodName)
        {
            var method = _type.GetMethod(methodName);
            if (method == null) throw new ArgumentException();
            return method.GetParameters().Select(p => $"{p.ParameterType.Name} {p.Name}");
        }

        public IEnumerable<string> GetAllFields() => _type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).Select(f => f.Name);

        public IEnumerable<string> GetProperties() => _type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).Select(p => p.Name);

        public bool HasAttribute<T>() where T : Attribute => _type.IsDefined(typeof(T), false);
    }
}
