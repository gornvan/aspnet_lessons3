using App.ServerPerformance;
using System.Text.Json;
using Xunit.Abstractions;

namespace App.Tests;
public class PerformanceStatsProviderTests
{
    private static int counter = 0;
    private readonly ITestOutputHelper output;

    public PerformanceStatsProviderTests(ITestOutputHelper output)
    {
        output.WriteLine($@"{counter++}");
        this.output = output;
    }

    [Fact]
    public void TestMinimalOperability_ValidateOutputs()
    {
        // arrange
        // nothing to arrange

        // act
        var stats = PerformanceStatsProvider.GetPerformanceStats();
        output.WriteLine(JsonSerializer.Serialize(stats));

        // assert
        Assert.True(stats != null);
        Assert.True(
            stats.OccupiedMemoryPercentage > 0 &&
            stats.OccupiedMemoryPercentage < 100);

        Assert.True(
            stats.TotalCpuUtilizationPercentage > 0 &&
            stats.TotalCpuUtilizationPercentage < 100);

        Assert.True(
            stats.TotalRunningProcessesCount > 0);
    }

    [Fact]
    public void TestSensibleAmountOfProcesses()
    {
        // arrange
        // nothing to arrange

        // act
        var stats = PerformanceStatsProvider.GetPerformanceStats();
        output.WriteLine(JsonSerializer.Serialize(stats));

        Assert.True(
            stats.TotalRunningProcessesCount > 20);
    }
}
