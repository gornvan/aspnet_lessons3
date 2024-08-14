namespace App.Tests;

public class MathTests
{
    [Theory]
    [InlineData(10, 3, 1000)]
    [InlineData(-1, 3, -1)]
    [InlineData(0, 3, 0)]
    [InlineData(0.5, 3.0, 125)]
    public void Power(double baseN, double powerN, double expected)
    {
        // arrange is done

        // act
        var result = Math.Pow(baseN, powerN);

        // assert
        Assert.Equal(expected, result);
    }
}

