using System.Diagnostics;

namespace App.PerformanceStatsProvider;
// Data class to hold performance statistics
public class PerformanceStats
{
    public float TotalCpuUtilizationPercentage { get; set; }
    public float OccupiedMemoryPercentage { get; set; }
    public int TotalRunningProcessesCount { get; set; }
}

// Class responsible for gathering performance statistics
public class PerformanceStatsProvider
{
    private IPerformanceCounter _cpuUtilizationCounter;
    private IPerformanceCounter _memoryUtilizationCounter;
    public PerformanceStatsProvider(IPerformanceCounter cpuUtilizationCounter, IPerformanceCounter memoryUtilizationCounter)
    {
        _cpuUtilizationCounter = cpuUtilizationCounter;
        _memoryUtilizationCounter = memoryUtilizationCounter;
    }

    public PerformanceStats GetPerformanceStats()
    {
        PerformanceStats stats = new PerformanceStats();

        // 1. Get total CPU utilization percentage
        stats.TotalCpuUtilizationPercentage = _cpuUtilizationCounter.NextValue();

        // 2. Get occupied memory percentage
        stats.OccupiedMemoryPercentage = _memoryUtilizationCounter.NextValue();

        // 3. Get total running processes count
        stats.TotalRunningProcessesCount = Process.GetProcesses().Length;

        return stats;
    }
}
