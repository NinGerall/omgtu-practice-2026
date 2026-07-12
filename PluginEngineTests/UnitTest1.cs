using System;
using System.Collections.Generic;
using Xunit;
using PluginEngine;
using PluginsBase;

namespace PluginEngineTests
{
    public class MockPluginA : IPlugin { public void Execute() {} }
    public class MockPluginB : IPlugin { public void Execute() {} }
    public class MockPluginC : IPlugin { public void Execute() {} }

    public class PluginEngineGraphTests
    {
        [Fact]
        public void TopologicalSort_ShouldOrderDependenciesCorrectly()
        {
            var plugins = new List<(Type Type, PluginLoadAttribute Attr)>
            {
                (typeof(MockPluginC), new PluginLoadAttribute("PluginC", "PluginB")),
                (typeof(MockPluginB), new PluginLoadAttribute("PluginB", "PluginA")),
                (typeof(MockPluginA), new PluginLoadAttribute("PluginA"))
            };

            var sorted = Program.TopologicalSort(plugins);

            Assert.Equal(3, sorted.Count);
            Assert.Equal(typeof(MockPluginA), sorted[0]);
            Assert.Equal(typeof(MockPluginB), sorted[1]);
            Assert.Equal(typeof(MockPluginC), sorted[2]);
        }

        [Fact]
        public void TopologicalSort_WithCyclicDependencies_ThrowsException()
        {
            var plugins = new List<(Type Type, PluginLoadAttribute Attr)>
            {
                (typeof(MockPluginA), new PluginLoadAttribute("PluginA", "PluginB")),
                (typeof(MockPluginB), new PluginLoadAttribute("PluginB", "PluginA"))
            };

            Assert.Throws<InvalidOperationException>(() => Program.TopologicalSort(plugins));
        }
    }
}