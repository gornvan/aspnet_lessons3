using App.ServerPerformance;
using System.Text.Json;
using Xunit.Abstractions;

namespace App.Tests;
public class PerformanceStatsProviderTests
{
    private readonly ITestOutputHelper output;

    public PerformanceStatsProviderTests(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void Test1()
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
}
