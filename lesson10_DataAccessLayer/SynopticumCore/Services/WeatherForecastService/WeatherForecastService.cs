using lesson8_WebApi;
using SynopticumCore.Contract.Interfaces.Queries;
using SynopticumCore.Contract.Interfaces.WeatherForecastService;
using SynopticumCore.Contract.Model;

namespace SynopticumCore.Services.WeatherForecastService
{
    public class WeatherForecastService : IWeatherForecastService
    {
        private static readonly string[] Summaries = new[]
            {
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };

        public async Task<IEnumerable<WeatherForecastDTO>> GetForecast(MultipleWeatherForecastQuery query)
        {
            var targetCity = Mocks.Cities
                .FirstOrDefault(
                    c => c.Name == query.CityName
                    && c.Country.Name == query.CountryName);

            if (targetCity == null)
            {
                throw new KeyNotFoundException("No such city");
            }

            await Task.Delay(100);

            return Enumerable.Range(0, 1024 * 1024 * 1024) /* Emulating a LARGE source of data which we will not consume in full thanks to Pagination */
                .Select(index =>
                new WeatherForecastDTO
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)],
                    City = targetCity.Name,
                    Country = targetCity.Country.Name,
                })
                // Always filter BEFORE paging
                .Where(forecast =>
                    ( query.MinTemperatureC == null || forecast.TemperatureC >= query.MinTemperatureC )
                    &&
                    ( query.MaxTemperatureC == null || forecast.TemperatureC <= query.MaxTemperatureC )
                    );
        }
    }
}
