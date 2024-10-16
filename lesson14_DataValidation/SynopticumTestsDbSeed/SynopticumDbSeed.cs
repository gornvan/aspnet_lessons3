using Microsoft.EntityFrameworkCore;
using SynopticumDAL.Services;
using SynopticumModel.Entities;
using SynopticumModel.Enums;

namespace SynopticumTestsDbSeed
{
    public class SynopticumDbSeed(
        SynopticumDbContext _dbcontext)
    {
        private const int DataCopies = 1;

        public async Task Seed()
        {
            var citySet = _dbcontext.Set<City>();
            var countrySet = _dbcontext.Set<Country>();
            var forecastsSet = _dbcontext.Set<WeatherForecast>();

            // Create countries
            var countries = new[]
            {
                new Country { Name = "Belarus" },
                new Country { Name = "Germany" },
                new Country { Name = "Brazil" },
                new Country { Name = "Japan" },
                new Country { Name = "Australia" }
            };

            // Create cities
            var cities = new[]
            {
                new City { Name = "Minsk", Country = countries[0] },
                new City { Name = "Hrodna", Country = countries[0] },
                new City { Name = "Berlin", Country = countries[1] },
                new City { Name = "Rio de Janeiro", Country = countries[2] },
                new City { Name = "Tokyo", Country = countries[3] },
                new City { Name = "Sydney", Country = countries[4] }
            };
            
            var random = new Random();

            for (var i = 0; i < DataCopies; i++)
            {
                foreach (var plannedCity in cities)
                {
                    var postfix = i > 0 ? i.ToString() : "";
                    var countryName = $@"{plannedCity.Country.Name}{postfix}";
                    var cityName = $"{plannedCity.Name}{postfix}";
                    var countryEntity = await countrySet
                        .FirstOrDefaultAsync(c => c.Name == countryName);
                    var cityEntity = await citySet
                        .FirstOrDefaultAsync(c => c.Name == cityName && c.Country.Name == countryName);

                    if (countryEntity == null)
                    {
                        countryEntity = new Country { Name = countryName };
                        countrySet.Add(countryEntity);
                    }

                    if (cityEntity == null)
                    {
                        cityEntity = new City { Name = cityName, Country = countryEntity };
                        citySet.Add(cityEntity);
                    }

                    for (int f = 0; f < 30; f++)
                    {
                        var forecast = new WeatherForecast
                        {
                            Date = DateOnly.FromDateTime(DateTime.Today.AddDays(f)),
                            TemperatureC = random.Next(-10, 40), // Random temperatures between -10°C and 40°C
                            Summary = (WeatherSummary)random.Next(1, 11), // Random summary from 1 to 10
                            City = cityEntity
                        };

                        if (!await forecastsSet
                            .AnyAsync(fc => fc.City.Name == plannedCity.Name && fc.City.Country.Name == countryName && fc.Date == forecast.Date))
                        {
                            forecastsSet.Add(forecast);
                        }
                    }

                    await _dbcontext.SaveChangesAsync();
                }
            }
            await _dbcontext.SaveChangesAsync();
        }
    }
}
