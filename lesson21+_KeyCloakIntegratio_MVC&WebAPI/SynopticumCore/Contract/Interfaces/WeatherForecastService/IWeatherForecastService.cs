using SynopticumCore.Contract.Queries.WeatherForecastQuery;

namespace SynopticumCore.Contract.Interfaces.WeatherForecastService
{
    public interface IWeatherForecastService : IService
    {
        Task<IQueryable<WeatherForecastResponse>> GetForecast(MultipleWeatherForecastQuery query);

        Task<NewWeatherForecastResponse> AddForecast(NewWeatherForecast newForecast);
    }
}
