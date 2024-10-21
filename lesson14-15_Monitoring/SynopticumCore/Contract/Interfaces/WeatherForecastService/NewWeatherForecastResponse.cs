using SynopticumModel.Enums;

namespace SynopticumCore.Contract.Interfaces.WeatherForecastService
{
    public class NewWeatherForecastResponse
    {
        public required int Id { get; set; }

        public required DateOnly Date { get; set; }

        public required int TemperatureC { get; set; }

        public required WeatherSummary Summary { get; set; }

        public required string CountryName { get; set; }

        public required string CityName { get; set; }
    }
}
