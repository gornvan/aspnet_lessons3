using App.PerformanceStatsProvider;
using System.Text.Json;
using Xunit.Abstractions;

namespace App.Tests;
public class PerformanceStatsProviderTests
{
    private static int counter = 0;
    private readonly ITestOutputHelper output;

    private Mock<IPerformanceCounter> _cpuPerformanceCounterMock;
    private Mock<IPerformanceCounter> _memoPerformanceCounterMock;

    private PerformanceStatsProvider.PerformanceStatsProvider _performanceStatsProvider;

    public PerformanceStatsProviderTests(ITestOutputHelper output)
    {
        // demonstration of the constructor being called for each Test - the counter will grow
        output.WriteLine($@"{counter++}");
        this.output = output;

        _cpuPerformanceCounterMock = new Mock<IPerformanceCounter>();
        _cpuPerformanceCounterMock
            .Setup(c => c.NextValue())
            .Returns(() => 42f);
        var cpuPerformanceCounter = _cpuPerformanceCounterMock.Object;

        _memoPerformanceCounterMock = new Mock<IPerformanceCounter>();
        _memoPerformanceCounterMock
            .Setup(c => c.NextValue())
            .Returns(() => 78f);
        var memoPerformanceCounter = _memoPerformanceCounterMock.Object;

        _performanceStatsProvider =
            new PerformanceStatsProvider.PerformanceStatsProvider(
                cpuPerformanceCounter,
                memoPerformanceCounter);
    }

    [Fact]
    public void ProviderReliesOnMemoryCounterToReturnOccupiedMemoryMetric()
    {
        // arrange
        // provider already initialized in the constructor

        // act
        var stats = _performanceStatsProvider.GetPerformanceStats();
        output.WriteLine(JsonSerializer.Serialize(stats));

        // assert
        _memoPerformanceCounterMock.Verify(c => c.NextValue(), Times.Once());
    }


    [Fact]
    public void ProviderReliesOnCpuCounterToReturnOccupiedCpuMetric()
    {
        // arrange
        // provider already initialized in the constructor

        // act
        var stats = _performanceStatsProvider.GetPerformanceStats();
        output.WriteLine(JsonSerializer.Serialize(stats));

        // assert
        _cpuPerformanceCounterMock.Verify(c => c.NextValue(), Times.Once());
    }

    [Fact]
    public void TestMinimalOperability_ValidateOutputs()
    {
        // arrange
        // nothing to arrange

        // act
        var stats = _performanceStatsProvider.GetPerformanceStats();
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
        var stats = _performanceStatsProvider.GetPerformanceStats();
        output.WriteLine(JsonSerializer.Serialize(stats));

        Assert.True(
            stats.TotalRunningProcessesCount > 20);
    }
}
