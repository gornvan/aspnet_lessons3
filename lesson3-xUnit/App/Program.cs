
// to dump, install dotnet dump tool

using ConsoleApp1;
using App.PerformanceStatsProvider;
using App.PerformanceStatsProvider.Windows;

Console.WriteLine("Hello, World!");

var a = 33;

var b = a * 2;

Test.DoStuff(b);


Console.WriteLine(b);


IPerformanceCounter cpuCounter;
IPerformanceCounter memoCounter;
if (OperatingSystem.IsWindows())
{
    cpuCounter = new CpuUtilizationPercentageCounter_Windows();
    memoCounter = new OccupiedMemoryPercentageCounter_Windows();
}
else
{
    throw new NotSupportedException("Only for Windows yet!");
}

var performanceProvider = new PerformanceStatsProvider(cpuCounter, memoCounter);

while (true)
{
    var stats = performanceProvider.GetPerformanceStats();
    Console.WriteLine("Total CPU Utilization: {0}%", stats.TotalCpuUtilizationPercentage);
    Console.WriteLine("Occupied Memory: {0}%", stats.OccupiedMemoryPercentage);
    Console.WriteLine("Total Running Processes: {0}", stats.TotalRunningProcessesCount);
    await Task.Delay(1000);
}