using Xunit;
using task14;

public class IntegralTests
{
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
}