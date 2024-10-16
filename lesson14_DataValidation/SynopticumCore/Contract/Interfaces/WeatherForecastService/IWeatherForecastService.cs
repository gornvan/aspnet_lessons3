using SynopticumCore.Contract.Queries.WeatherForecastQuery;

namespace SynopticumCore.Contract.Interfaces.WeatherForecastService
{
    public interface IWeatherForecastService : IService
    {
        Task<IQueryable<WeatherForecastDTO>> GetForecast(MultipleWeatherForecastQuery query);
    }
}
