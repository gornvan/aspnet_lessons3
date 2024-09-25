using SynopticumCore.Contract.Interfaces.Queries;
using SynopticumCore.Contract.Interfaces.WeatherForecastService;
using SynopticumCore.Services.WeatherForecastService;

namespace SynopticumCoreTests.Services
{
    public class WeatherForecastServiceTests
    {
        private IWeatherForecastService weatherForecastService;
        public WeatherForecastServiceTests()
        {
            // arrange
            weatherForecastService = new WeatherForecastService();
        }

        [Theory]
        [InlineData("Belarus", "Hrodna", -100, 100)]
        [InlineData("Vatican", "Vatican", -100, 100)]
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
            Assert.Equal(DateOnly.FromDateTime(DateTime.Now), firstForecast.Date);
        }
    }
}