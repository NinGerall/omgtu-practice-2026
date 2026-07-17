using System;
using ScottPlot;

public class Program
{
    public static void Main()
    {
        var plt = new ScottPlot.Plot();
        
        plt.Title("Циклическое квантование времени (Round-Robin Slicing)");
        plt.XLabel("Логическое время (тики потока)");
        plt.YLabel("Идентификатор команды TestCommand");

        double[,] executions = {
            { 1, 0, 1 }, { 2, 1, 1 }, { 3, 2, 1 }, { 4, 3, 1 }, { 5, 4, 1 },
            { 1, 5, 1 }, { 2, 6, 1 }, { 3, 7, 1 }, { 4, 8, 1 }, { 5, 9, 1 },
            { 1, 10, 1 }, { 2, 11, 1 }, { 3, 12, 1 }, { 4, 13, 1 }, { 5, 14, 1 }
        };

        var colors = new ScottPlot.Color[] {
            ScottPlot.Colors.Blue,
            ScottPlot.Colors.Green,
            ScottPlot.Colors.Red,
            ScottPlot.Colors.Orange,
            ScottPlot.Colors.Purple
        };

        for (int i = 0; i < executions.GetLength(0); i++)
        {
            int taskId = (int)executions[i, 0];
            double start = executions[i, 1];
            double duration = executions[i, 2];

            double yMin = taskId - 0.4;
            double yMax = taskId + 0.4;
            double xMin = start;
            double xMax = start + duration;

            var rect = plt.Add.Rectangle(xMin, xMax, yMin, yMax);
            rect.FillStyle.Color = colors[(taskId - 1) % colors.Length];
            rect.LineStyle.Width = 1;
            rect.LineStyle.Color = ScottPlot.Colors.White;
        }

        plt.Axes.SetLimits(-0.5, 15.5, 0.5, 5.5);

        double[] tickPositions = { 1, 2, 3, 4, 5 };
        string[] tickLabels = { "T1", "T2", "T3", "T4", "T5" };
        plt.Axes.Left.SetTicks(tickPositions, tickLabels);

        plt.Grid.MajorLineColor = ScottPlot.Color.FromHex("#20000000");

        plt.SavePng("benchmark_RR.png", 700, 450);
        Console.WriteLine("График успешно сгенерирован под ScottPlot 5!");
    }
}