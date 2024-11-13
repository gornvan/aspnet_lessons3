using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SynopticumWebAPI.Controllers
{
    [ApiController]
    [Route("links")]
    public class LinksController : ControllerBase
    {
        [HttpGet("LinkToDefaultTemperatureProbe")]
        public string GetLinkToProbe()
        {
            var routeValues = new
            {
                //coordinates = "53.9,27.55",
                probeType = "Temperature"
            };
            
            var url = Url.RouteUrl("probes",
                                values: routeValues)!;

            return url;
        }


        [HttpGet("LinkToWeatherForecastAddingEndpoint")]
        public string GetLinkToForecast()
        {
            var url = Url.Action("Post", "WeatherForecast")!;

            return url;
        }
    }
}
