using System;

namespace task07
{
    // Этот атрибут применяется только к классам
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class VersionAttribute : Attribute
    {
        public string Version { get; }

        public VersionAttribute(string version)
        {
            Version = version ?? throw new ArgumentNullException(nameof(version));
        }
    }
}