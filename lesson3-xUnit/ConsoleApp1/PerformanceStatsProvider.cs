using System.Diagnostics;

namespace ServerPerformance
{
    // Data class to hold performance statistics
    public class PerformanceStats
    {
        public int TotalCpuUtilizationPercentage { get; set; }
        public int OccupiedMemoryPercentage { get; set; }
        public int TotalRunningProcessesCount { get; set; }
    }

    // Class responsible for gathering performance statistics
    public static class PerformanceStatsProvider
    {
        public static PerformanceStats GetPerformanceStats()
        {
            PerformanceStats stats = new PerformanceStats();

            // 1. Get total CPU utilization percentage
            using (PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total"))
            {
                // We need to call NextValue() twice to get a valid reading
                cpuCounter.NextValue();
                System.Threading.Thread.Sleep(1000);  // Sleep one second to get a stable reading
                stats.TotalCpuUtilizationPercentage = (int)cpuCounter.NextValue();
            }

            // 2. Get occupied memory percentage
            using (PerformanceCounter memoryCounter = new PerformanceCounter("Memory", "% Committed Bytes In Use"))
            {
                stats.OccupiedMemoryPercentage = (int)memoryCounter.NextValue();
            }

            // 3. Get total running processes count
            stats.TotalRunningProcessesCount = Process.GetProcesses().Length;

            return stats;
        }
    }
}