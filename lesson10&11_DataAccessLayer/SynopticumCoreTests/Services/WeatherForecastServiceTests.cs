using SynopticumCore.Contract.Interfaces.WeatherForecastService;
using SynopticumCore.Contract.Queries.WeatherForecastQuery;
using SynopticumCore.Services.WeatherForecastService;
using Xunit.Abstractions;

namespace SynopticumCoreTests.Services
{
    public class WeatherForecastServiceTests
    {
        private IWeatherForecastService weatherForecastService;
        private readonly ITestOutputHelper _output;

        public WeatherForecastServiceTests()
        {
            // arrange
            var uow = UoWInitializer.Initialize();
            weatherForecastService = new WeatherForecastService(uow);
        }

        [Theory]
        [InlineData("Belarus", "Hrodna", -100, 100)]
        [InlineData("Australia", "Sydney", -100, 100)]
        public async Task GetAtLeastOneFilledWeatherForecast(
            string countryName,
            string? cityName,
            int? minTemperatureC,
            int? maxTemperatureC)
        {
            // arrange
            var query = new MultipleWeatherForecastQuery
            {
                CityName = cityName,
                CountryName = countryName,
                MaxTemperatureC = maxTemperatureC,
                MinTemperatureC = minTemperatureC,
            };

            // act
            var results = await weatherForecastService.GetForecast(query);

            // assert
            var firstForecast = results.First();
            Assert.True(firstForecast.TemperatureF > int.MinValue);
            Assert.True(firstForecast.TemperatureC > int.MinValue);
            Assert.Equal(cityName, firstForecast.City);
            Assert.Equal(countryName, firstForecast.Country);
            Assert.NotNull(firstForecast.Date); // TODO: actually generate new forecasts every day within seed?
        }
    }
}