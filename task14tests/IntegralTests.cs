using Xunit;
using Xunit.Abstractions;
using task14;
using System.Diagnostics;
using ScottPlot;

public class IntegralTests
{
    private readonly ITestOutputHelper _output;

    public IntegralTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void TestLinearSymmetric()
    {
        double result = DefiniteIntegral.Solve(-1, 1, x => x, 1e-4, 2);
        Assert.Equal(0, result, 4);
    }

    [Fact]
    public void TestSinSymmetric()
    {
        double result = DefiniteIntegral.Solve(-1, 1, x => Math.Sin(x), 1e-5, 8);
        Assert.Equal(0, result, 4);
    }

    [Fact]
    public void TestLinearPositive()
    {
        double result = DefiniteIntegral.Solve(0, 5, x => x, 1e-6, 8);
        Assert.Equal(12.5, result, 5);
    }

    [Fact]
    public void RunFullBenchmark()
    {
        const double a = -100;
        const double b = 100;
        const double step = 1e-5;
        const int iterations = 10;
        Func<double, double> func = x => Math.Sin(x);

        // 1. Замер однопоточной версии
        double sequentialSumMs = 0;
        for (int i = 0; i < iterations; i++)
        {
            var sw = Stopwatch.StartNew();
            DefiniteIntegral.SolveSequential(a, b, func, step);
            sw.Stop();
            sequentialSumMs += sw.Elapsed.TotalMilliseconds;
        }
        double seqAvgTime = sequentialSumMs / iterations;

        // 2. Замеры многопоточной версии (от 1 до 16 потоков)
        int[] threadCounts = { 1, 2, 4, 8, 12, 16 };
        double[] avgTimes = new double[threadCounts.Length];

        for (int t = 0; t < threadCounts.Length; t++)
        {
            int threads = threadCounts[t];
            double multiSumMs = 0;
            for (int i = 0; i < iterations; i++)
            {
                var sw = Stopwatch.StartNew();
                DefiniteIntegral.Solve(a, b, func, step, threads);
                sw.Stop();
                multiSumMs += sw.Elapsed.TotalMilliseconds;
            }
            avgTimes[t] = multiSumMs / iterations;
        }

        // 3. Строим график в ScottPlot
        var plt = new Plot();
        plt.Title("Зависимость времени вычислений от количества потоков");
        
        // Преобразуем данные для осей: OX - время (ms), OY - потоки
        double[] ox = avgTimes;
        double[] oy = threadCounts.Select(x => (double)x).ToArray();

        var scatter = plt.Add.Scatter(ox, oy);
        scatter.Color = Colors.RoyalBlue;
        scatter.LineWidth = 3;
        scatter.MarkerSize = 10;

        plt.XLabel("Время выполнения (мс)");
        plt.YLabel("Количество потоков");

        // Сохраняем график на уровень выше (в корень проекта)
        string plotPath = Path.Combine("..", "..", "..", "..", "benchmark.png");
        plt.SavePng(plotPath, 800, 600);

        // 4. Формируем текстовый отчет
        double bestMultiTime = avgTimes.Min();
        int bestThreads = threadCounts[Array.IndexOf(avgTimes, bestMultiTime)];
        double differencePct = ((seqAvgTime - bestMultiTime) / seqAvgTime) * 100;

        string reportPath = Path.Combine("..", "..", "..", "..", "benchmark_report.txt");
        using (var writer = new StreamWriter(reportPath))
        {
            writer.WriteLine("=== ОТЧЕТ ПО ОПТИМИЗАЦИИ И СРАВНЕНИЮ ПРОИЗВОДИТЕЛЬНОСТИ ===");
            writer.WriteLine($"Выбранный размер шага: {step:G}");
            writer.WriteLine($"Среднее время однопоточной версии: {seqAvgTime:F4} мс");
            writer.WriteLine("---------------------------------------------------------");
            for (int t = 0; t < threadCounts.Length; t++)
            {
                writer.WriteLine($"Потоков: {threadCounts[t]} | Среднее время: {avgTimes[t]:F4} мс");
            }
            writer.WriteLine("---------------------------------------------------------");
            writer.WriteLine($"Оптимальное количество потоков: {bestThreads}");
            writer.WriteLine($"Время лучшей многопоточной версии: {bestMultiTime:F4} мс");
            writer.WriteLine($"Ускорение многопоточной версии по сравнению с однопоточной: {differencePct:F2}%");
        }
    }
}