namespace SynopticumCore.Contract.Model
{
    public class WeatherForecast
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

        public required City City { get; set; }
    }
}
