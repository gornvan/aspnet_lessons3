using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using SynopticumCore.Contract.Queries.WeatherForecastQuery;
using SynopticumCore.Contract.Interfaces.WeatherForecastService;

namespace lesson8_WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController(
        IWeatherForecastService _weatherForecastService,
        ILogger<WeatherForecastController> _logger
        ) : ControllerBase
    {

        /// <summary>
        /// Example of a RESTful endpoint allowing to filter, paginate and project the data
        /// </summary>
        /// <returns></returns>
        [HttpGet(
            template:"countries/{countryName}/cities/{cityName}",
            Name = "GetWeatherForecast")]
        public async Task<IActionResult> Get(
            string countryName,
            string cityName,
            int minTemperatureC,
            int maxTemperatureC,
            int pageSize = 5,
            int pageNumber = 1,
            [FromQuery] string[]? fields = null)
        {
            // prepare query for the service
            var query = new MultipleWeatherForecastQuery
            {
                CityName = cityName,
                CountryName = countryName,
                MaxTemperatureC = maxTemperatureC,
                MinTemperatureC = minTemperatureC,
            };

            try
            {
                // call the service
                var serviceQuery = await _weatherForecastService.GetForecast(query);

                // apply pagination and projection
                var paginatedQuery = serviceQuery  // Always page AFTER filtering
                    .Skip(pageSize * (pageNumber - 1))
                    .Take(pageSize);

                var projectedQuery = fields == null
                    ? paginatedQuery
                    : paginatedQuery.Select(forecast => MutateObjectToGetProjection(forecast, fields));

                return new JsonResult(projectedQuery.ToList());
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Unexpected error, please try again or contact support, sorry for the inconvenience");
            }

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
        public async Task<IActionResult> Get(
            string countryName,
            string cityName,
            int daysAhead)
        {
            // prepare query for the service
            var query = new MultipleWeatherForecastQuery
            {
                CityName = cityName,
                CountryName = countryName,
            };

            try
            {
                // call the service
                var serviceQuery = await _weatherForecastService.GetForecast(query);

                // apply pagination
                var forecast = serviceQuery  // Always page AFTER filtering
                    .Skip(daysAhead)
                    .First();

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
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Unexpected error, please try again or contact support, sorry for the inconvenience");
            }

        }
    }
}
