using SynopticumCore.Contract.Interfaces.Queries;

namespace SynopticumCore.Contract.Interfaces.WeatherForecastService
{
    public interface IWeatherForecastService : IService
    {
        Task<IEnumerable<WeatherForecastDTO>> GetForecast(MultipleWeatherForecastQuery query);
    }
}
