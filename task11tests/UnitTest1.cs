using System;
using Xunit;
using task11;

namespace task11tests
{
    public class CalculatorGenerationTests
    {
        private const string TargetSource = @"
        public class Calculator
        {
            public int Add(int a, int b) => a + b;
            public int Minus(int a, int b) => a - b;
            public int Mul(int a, int b) => a * b;
            public int Div(int a, int b) => a / b;
        }";

        [Fact]
        public void DynamicCalculator_ShouldCompileAndExecuteCorrectly()
        {
            ICalculator calc = CompilerService.CompileCalculator(TargetSource);

            Assert.NotNull(calc);
            Assert.Equal(15, calc.Add(10, 5));
            Assert.Equal(5, calc.Minus(10, 5));
            Assert.Equal(50, calc.Mul(10, 5));
            Assert.Equal(2, calc.Div(10, 5));
        }
    }
}