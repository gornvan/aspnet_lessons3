using Microsoft.EntityFrameworkCore;
using SynopticumDAL.Contract;
using SynopticumModel.Entities;
using System.Diagnostics;
using Xunit.Abstractions;

namespace SynopticumCoreTests.Performance;

[Collection("Don't run in parallel")]
public class SearchForCountryPerfTest
{

    private IUnitOfWork _unitOfWork;
    private readonly ITestOutputHelper _output;

    public SearchForCountryPerfTest(ITestOutputHelper output)
    {
        _unitOfWork = UoWInitializer.Initialize();
        _output = output;
    }

    [Fact]
    public async Task MeasureCountrySearchPerformance()
    {
        var countryRepo = _unitOfWork.GetRepository<Country>();
        var countryNames = new[] { "Belarus", "Germany", "Brazil", "Japan", "Australia" };
        long totalTicks = 0;
        int iterations = 500;

        // warmup
        for (int i = 0; i < iterations; i++)
        {
            foreach (var name in countryNames)
            {
                var country = await countryRepo.AsReadOnlyQueryable().FirstOrDefaultAsync(c => c.Name.StartsWith(name.Substring(0, 3)));
            }
        }

        for (int i = 0; i < iterations; i++)
        {
            foreach (var name in countryNames)
            {
                var stopwatch = Stopwatch.StartNew();

                var country = await countryRepo.AsReadOnlyQueryable().FirstOrDefaultAsync(c => c.Name.StartsWith(name.Substring(0, 3)));

                stopwatch.Stop();
                totalTicks += stopwatch.ElapsedTicks;
            }
        }

        var totalSearches = iterations * countryNames.Length;
        var averageTime = totalTicks / (iterations * countryNames.Length);
        _output.WriteLine($"Average time to search ({totalSearches} searches): {averageTime} ticks");

        Assert.True(averageTime < 10);
    }
}