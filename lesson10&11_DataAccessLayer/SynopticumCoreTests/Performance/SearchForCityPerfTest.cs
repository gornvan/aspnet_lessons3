using Microsoft.EntityFrameworkCore;
using SynopticumDAL.Contract;
using SynopticumModel.Entities;
using System.Diagnostics;
using Xunit.Abstractions;

namespace SynopticumCoreTests.Performance
{
    [Collection("Don't run in parallel")]
    public class SearchForCityPerfTest
    {
        private IUnitOfWork _unitOfWork;
        private readonly ITestOutputHelper _output;
        public SearchForCityPerfTest(ITestOutputHelper output)
        {
            // arrange
            _unitOfWork = UoWInitializer.Initialize();
            _output = output;
        }

        [Fact]
        public async Task MeasureCitySearchPerformance()
        {
            var cityRepo = _unitOfWork.GetRepository<City>();
            var cityNames = new[] { "Minsk", "Hrodna", "Berlin", "Rio de Janeiro", "Tokyo", "Sydney" };
            long totalTicks = 0;
            int iterations = 500;

            // warmup
            for (int i = 0; i < iterations; i++)
            {
                foreach (var name in cityNames)
                {
                    var city = await cityRepo.AsReadOnlyQueryable().FirstOrDefaultAsync(c => c.Name.StartsWith(name.Substring(0, 3)));
                }
            }

            for (int i = 0; i < iterations; i++)
            {
                foreach (var name in cityNames)
                {
                    var stopwatch = Stopwatch.StartNew();

                    var city = await cityRepo.AsReadOnlyQueryable().FirstOrDefaultAsync(c => c.Name.StartsWith(name.Substring(0, 3)));

                    stopwatch.Stop();
                    totalTicks += stopwatch.ElapsedTicks;
                }
            }

            var totalSearches = iterations * cityNames.Length;
            var averageTime = totalTicks / (iterations * cityNames.Length);
            _output.WriteLine($"Average time to search ({totalSearches} searches): {averageTime} ticks");

            Assert.True(averageTime < 100);
        }
    }
}
