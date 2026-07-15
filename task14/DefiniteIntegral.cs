namespace task14;

public static class DefiniteIntegral
{
    private static double _totalSum;

    // Многопоточная реализация
    public static double Solve(double a, double b, Func<double, double> function, double step, int threadsNumber)
    {
        _totalSum = 0.0;

        using var barrier = new Barrier(threadsNumber + 1);

        double segmentWidth = (b - a) / threadsNumber;

        for (int i = 0; i < threadsNumber; i++)
        {
            int index = i;
            var thread = new Thread(() =>
            {
                double localStart = a + index * segmentWidth;
                double localEnd = localStart + segmentWidth;
                double localSum = CalculateSegment(localStart, localEnd, function, step);

                Accumulate(localSum);

                barrier.SignalAndWait();
            });
            thread.Start();
        }

        barrier.SignalAndWait();

        return _totalSum;
    }

    // Однопоточная последовательная реализация (без потоков вообще)
    public static double SolveSequential(double a, double b, Func<double, double> function, double step)
    {
        return CalculateSegment(a, b, function, step);
    }

    private static double CalculateSegment(double start, double end, Func<double, double> function, double step)
    {
        double sum = 0.0;
        double current = start;

        while (current < end)
        {
            double next = Math.Min(current + step, end);
            sum += 0.5 * (function(current) + function(next)) * (next - current);
            current = next;
        }

        return sum;
    }

    private static void Accumulate(double value)
    {
        double initial, computed;
        do
        {
            initial = _totalSum;
            computed = initial + value;
        } 
        while (Interlocked.CompareExchange(ref _totalSum, computed, initial) != initial);
    }
}