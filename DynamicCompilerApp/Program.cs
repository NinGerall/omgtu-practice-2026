using System;
using DynamicCalcBase;

namespace DynamicCompilerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string calculatorSource = @"
            public class Calculator
            {
                public int Add(int a, int b) => a + b;
                public int Minus(int a, int b) => a - b;
                public int Mul(int a, int b) => a * b;
                public int Div(int a, int b) => a / b;
            }";

            try
            {
                ICalculator calc = CompilerService.CompileCalculator(calculatorSource);

                Console.WriteLine($"Вызов методов БЕЗ рефлексии:");
                Console.WriteLine($"Add(10, 5)   = {calc.Add(10, 5)}");
                Console.WriteLine($"Minus(10, 5) = {calc.Minus(10, 5)}");
                Console.WriteLine($"Mul(10, 5)   = {calc.Mul(10, 5)}");
                Console.WriteLine($"Div(10, 5)   = {calc.Div(10, 5)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
        }
    }
}
