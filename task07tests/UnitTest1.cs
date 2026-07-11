using System;
using Xunit;
using task07;

namespace task07tests
{
    public class ReflectionHelperTests
    {
        [Fact]
        public void PrintTypeInfo_ValidType_ReturnsExpectedContent()
        {
            var result = ReflectionHelper.PrintTypeInfo(typeof(SampleClass));

            Assert.NotNull(result);
            Assert.Contains("Класс: SampleClass", result);
            Assert.Contains("Понятное имя класса: Демонстрационный Класс", result);
            Assert.Contains("Версия класса: 1.0", result);
            Assert.Contains("Идентификатор пользователя", result);
            Assert.Contains("Calculate", result);
            Assert.Contains("Выполнить расчет", result);
        }

        [Fact]
        public void PrintTypeInfo_NullType_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReflectionHelper.PrintTypeInfo(null));
        }
    }
}
