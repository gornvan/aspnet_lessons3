using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerGen;
using SynopticumCore.Contract.Interfaces.WeatherForecastService;
using SynopticumCore.Contract.Queries.WeatherForecastQuery;
using SynopticumCore.Validation.WeatherForecast;
using SynopticumWebAPI.Models.WeatherForecast;
using System.Text.Json;
using ILogger = Serilog.ILogger;

namespace lesson8_WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController(
        IWeatherForecastService _weatherForecastService,
        ILogger _logger
        ) : ControllerBase
    {

        [HttpPost(template: "countries/{CountryName}/cities/{CityName}",
            Name = "AddWeatherForecast")]
        [Authorize]
        public async Task<IActionResult> Post(NewWeatherForecastDTO forecastDto)
        {
            var newForecast = new NewWeatherForecast
            {
                CityName = forecastDto.CityName,
                CountryName = forecastDto.CountryName,
                Date = forecastDto.Date,
                Summary = forecastDto.Summary,
                TemperatureC = forecastDto.TemperatureC,
            };

            var validator = new NewWeatherForecastValidator();
            var validationResult = validator.Validate(newForecast);
            if (!validationResult.IsValid)
            {
                var errorsText = JsonSerializer.Serialize(validationResult.Errors);
                var sourceIp = Request.HttpContext.Connection.RemoteIpAddress;
                _logger.Warning(
                    $"A badly formed Forecast has been submitted by {sourceIp}; errors: {errorsText}; forecastDto: {JsonSerializer.Serialize(forecastDto)}");
                return BadRequest(errorsText);
            }

            var createdForecast = await _weatherForecastService.AddForecast(newForecast);

            return new JsonResult(createdForecast);
        }

        [HttpGet("Error")]
        public IActionResult GetError()
        {
            return new StatusCodeResult(statusCode: StatusCodes.Status500InternalServerError);
        }

        /// <summary>
        /// Example of a RESTful endpoint allowing to filter, paginate and project the data
        /// </summary>
        /// <returns></returns>
        [HttpGet(
            template: "countries/{countryName}/cities/{cityName}",
            Name = "GetWeatherForecast")]
        public async Task<IActionResult> Get(
            string countryName,
            string cityName,
            int? minTemperatureC,
            int? maxTemperatureC,
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
                var paginatedQuery = await serviceQuery  // Always page AFTER filtering
                    .Skip(pageSize * (pageNumber - 1))
                    .Take(pageSize)
                    .ToListAsync();

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

        private static T? MutateObjectToGetProjection<T>(T? originalObject, string[] fieldNames) where T : class
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
