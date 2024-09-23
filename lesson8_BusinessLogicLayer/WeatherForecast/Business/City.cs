using System.ComponentModel.DataAnnotations;

namespace WeatherForecast.Business
{
    public class City
    {
        [MaxLength(200)]
        public required string Name { get; set; }

        public required Country Country { get; set; }
    }
}
