using SynopticumCore.Contract.Interfaces.Queries.WeatherForecastQuery;

namespace SynopticumCore.Contract.Interfaces.WeatherForecastService
{
    public interface IWeatherForecastService : IService
    {
        Task<IEnumerable<WeatherForecastDTO>> GetForecast(MultipleWeatherForecastQuery query);
    }
}
