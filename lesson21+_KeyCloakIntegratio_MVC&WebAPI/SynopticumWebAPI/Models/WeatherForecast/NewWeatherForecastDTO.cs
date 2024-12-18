using SynopticumModel.Enums;
using System.ComponentModel.DataAnnotations;

namespace SynopticumWebAPI.Models.WeatherForecast
{
    public class NewWeatherForecastDTO
    {
        public required DateOnly Date { get; set; }

        [Range(-273, int.MaxValue)]
        public required int TemperatureC { get; set; }

        public required WeatherSummary Summary { get; set; }
    }
}
