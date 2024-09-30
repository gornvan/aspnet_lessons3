namespace SynopticumCore.Contract.Interfaces.Queries
{
    public class MultipleWeatherForecastQuery
    {
        public string CountryName { get; set; }
        public string? CityName { get; set; }
        public int? MinTemperatureC { get; set; }
        public int? MaxTemperatureC { get; set; }
    }
}
