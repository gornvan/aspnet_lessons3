using SynopticumCore.Contract.Interfaces.Queries;

namespace SynopticumCore.Contract.Interfaces.WeatherForecastService
{
    public interface IWeatherForecastService
    {
        Task<IEnumerable<WeatherForecastDTO>> GetForecast(MultipleWeatherForecastQuery query);
    }
}
