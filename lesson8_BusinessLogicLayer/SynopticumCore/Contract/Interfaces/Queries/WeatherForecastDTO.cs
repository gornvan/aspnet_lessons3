namespace SynopticumCore.Contract.Interfaces.Queries
{
    public class WeatherForecastDTO
    {
        public required DateOnly? Date { get; set; }

        public required int TemperatureC { get; set; }

        public int TemperatureF
        {
            get
            {
                return 32 + (int)(TemperatureC / 0.5556);
            }
        }

        public required string Summary { get; set; }

        public required string City { get; set; }
        public required string Country { get; set; }
    }
}
