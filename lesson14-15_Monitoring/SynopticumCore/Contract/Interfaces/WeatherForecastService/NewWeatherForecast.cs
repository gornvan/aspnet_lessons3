using SynopticumModel.Enums;
using System.ComponentModel.DataAnnotations;

namespace SynopticumCore.Contract.Interfaces.WeatherForecastService
{
    public class NewWeatherForecast
    {
        public required DateOnly Date { get; set; }

        [Range(-273, int.MaxValue)]
        public required int TemperatureC { get; set; }

        public required WeatherSummary Summary { get; set; }

        [MaxLength(100)]
        public required string CountryName { get; set; }
        [MaxLength(100)]
        public required string CityName { get; set; }
    }
}
