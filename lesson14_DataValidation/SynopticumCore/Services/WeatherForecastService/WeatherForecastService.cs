using Microsoft.EntityFrameworkCore;
using SynopticumCore.Contract.Interfaces.WeatherForecastService;
using SynopticumCore.Contract.Queries.WeatherForecastQuery;
using SynopticumDAL.Contract;
using SynopticumModel.Entities;

namespace SynopticumCore.Services.WeatherForecastService
{
    public class WeatherForecastService (IUnitOfWork _unitOfWork): IWeatherForecastService
    {
        public async Task<IQueryable<WeatherForecastDTO>> GetForecast(MultipleWeatherForecastQuery query)
        {
            var cityRepo = _unitOfWork.GetRepository<City>();
            var countryRepo = _unitOfWork.GetRepository<Country>();

            City? targetCity = null;
            Country? targetCountry = null;

            // if city is requested, check if it exists
            if (query.CityName != null)
            {
                targetCity = await cityRepo
                    .AsReadOnlyQueryable()
                    .FirstOrDefaultAsync(
                        c => c.Name == query.CityName
                        && c.Country.Name == query.CountryName);

                if (targetCity == null)
                {
                    throw new KeyNotFoundException("No such city");
                }
            }

            if (targetCity == null)
            {
                throw new KeyNotFoundException("No such city");
            }

            targetCountry = await countryRepo
                .AsReadOnlyQueryable()
                .FirstOrDefaultAsync(
                    c => c.Name == query.CountryName);

            if (targetCountry == null)
            {
                throw new KeyNotFoundException("No such country");
            }

            var forecastRepo = _unitOfWork.GetRepository<WeatherForecast>();
            var repoQuery = forecastRepo
                .AsReadOnlyQueryable()
                .Include(f => f.City);

            // filter by temperature
            var repoQueryWithFilter = repoQuery
                .Where(forecast =>
                    (query.MinTemperatureC == null || forecast.TemperatureC >= query.MinTemperatureC)
                    &&
                    (query.MaxTemperatureC == null || forecast.TemperatureC <= query.MaxTemperatureC)
                    );

            // filter by CityId or by country
            if (targetCity != null)
            {
                repoQueryWithFilter = repoQueryWithFilter
                    .Where(forecast => forecast.City.Id == targetCity.Id);
            }
            else
            {
                // filter by country
                repoQueryWithFilter = repoQueryWithFilter
                    .Where(forecast => query.CityName == null || forecast.City.Country.Id == targetCountry.Id);
            }

            return repoQueryWithFilter
                .Select(forecast =>
                new WeatherForecastDTO
                {
                    Date = forecast.Date,
                    TemperatureC = forecast.TemperatureC,
                    Summary = forecast.Summary.ToString(),
                    City = forecast.City.Name,
                    Country = query.CountryName,
                });
        }
    }
}
