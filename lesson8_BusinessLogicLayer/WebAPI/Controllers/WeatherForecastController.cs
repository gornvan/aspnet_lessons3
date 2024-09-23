using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using Synopticum.Business;

namespace lesson8_WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Example of a RESTful endpoint allowing to filter, paginate and project the data
        /// </summary>
        /// <returns></returns>
        [HttpGet(
            template:"countries/{countryName}/cities/{cityName}",
            Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast?> Get(
            string countryName,
            string cityName,
            int minTemperatureC,
            int maxTemperatureC,
            int pageSize = 5,
            int pageNumber = 1,
            [FromQuery] string[]? fields = null)
        {
            var targetCity = Mocks.Cities.First(c => c.Name == cityName && c.Country.Name == countryName);

            var unprojectedQuery = Enumerable.Range(1, 1024 * 1024 * 1024 /* Emulating a LARGE source of data which we will not consume in full thanks to Pagination */)
                .Select(index =>
                new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)],
                    City = targetCity
                })
                // Always filter BEFORE paging
                .Where(forecast =>
                    forecast.TemperatureC >= minTemperatureC
                    &&
                    forecast.TemperatureC <= maxTemperatureC)
                // Always page AFTER filtering
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize);

            var projectedQuery = fields == null
                ? unprojectedQuery
                : unprojectedQuery.Select(forecast => MutateObjectToGetProjection(forecast, fields));

            return projectedQuery.ToList();
        }

        private static T? MutateObjectToGetProjection<T>(T? originalObject, string[] fieldNames) where T: class
        {
            if (originalObject == default(T))
            {
                return originalObject;
            }

            var props = originalObject.GetType().GetProperties().Where(p => p.IsPubliclyWritable());

            foreach (var prop in props)
            {
                if (!fieldNames.Any(
                    fn => prop.Name.Equals(fn, StringComparison.OrdinalIgnoreCase))
                )
                {
                    prop.SetValue(originalObject, null);
                }
            }
            return originalObject;
        }

        /// <summary>
        /// Allow the client to request the forecase in a certain DATA TYPE via ACCEPT header
        /// </summary>
        /// <param name="daysAhead">The day of interest</param>
        /// <returns></returns>
        [Produces("application/json", new[] { "text/plain" })]
        [HttpGet(template: "countries/{countryName}/cities/{cityName}/futureDay/{daysAhead}", Name = "GetWeathForecastForConcreteDayAhead")]
        public IActionResult Get(
            string countryName,
            string cityName,
            int daysAhead)
        {
            var targetCity = Mocks.Cities.First(c => c.Name == cityName && c.Country.Name == countryName);

            var forecast = new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(daysAhead)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)],
                City = targetCity
            };

            var accept = Request.GetTypedHeaders().Accept;
            switch (accept[0].MediaType.ToString())
            {
                case "application/json":
                case "*/*":
                default:
                    return new JsonResult(forecast);

                case "text/plain":
                    return Content(
                        $"""
                        Date: {forecast.Date};
                        The temperature will be {forecast.TemperatureC} Celcius;
                        It will feel {forecast.Summary}.
                        """
                        );
            }
        }
    }
}
