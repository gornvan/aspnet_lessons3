
// to dump, install dotnet dump tool

using ConsoleApp1;
using App.ServerPerformance;

Console.WriteLine("Hello, World!");

var a = 33;

var b = a * 2;

Test.DoStuff(b);


Console.WriteLine(b);

while (true)
{
    var stats = PerformanceStatsProvider.GetPerformanceStats();
    Console.WriteLine("Total CPU Utilization: {0}%", stats.TotalCpuUtilizationPercentage);
    Console.WriteLine("Occupied Memory: {0}%", stats.OccupiedMemoryPercentage);
    Console.WriteLine("Total Running Processes: {0}", stats.TotalRunningProcessesCount);
    await Task.Delay(1000);
}