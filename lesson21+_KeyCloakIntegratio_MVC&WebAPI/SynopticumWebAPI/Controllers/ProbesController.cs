using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SynopticumWebAPI.Controllers
{
    [ApiController]
    [Route("probes/{coordinates=53.9,27.55}")]
    public class ProbesController : ControllerBase
    {
        [HttpGet("Temperature")]
        public string GetTemperature( string coordinates)
        {
            return $@"{coordinates} - Temperature: {new Random().NextDouble() * 100 - 50}";
        }

        [HttpGet("Humidity")]
        public string GetHumidity(string coordinates)
        {
            return $@"{coordinates} - Humidity: {new Random().NextDouble() * 100 - 50}";
        }

    }
}
