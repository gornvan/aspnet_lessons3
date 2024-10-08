﻿using Microsoft.EntityFrameworkCore;
using SynopticumDAL.Services;
using SynopticumModel.Entities;
using SynopticumModel.Enums;

namespace SynopticumDAL.Seed
{
    public class SynopticumDbSeed(
        SynopticumDbContext _dbcontext)
    {
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

            foreach (var country in countries)
            {
                if (!await countrySet.AnyAsync(c => c.Name == country.Name))
                {
                    await countrySet.AddAsync(country);
                }
            }

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

            foreach (var city in cities)
            {
                if (!await citySet.AnyAsync(c => c.Name == city.Name))
                {
                    await citySet.AddAsync(city);
                }
            }

            // Create weather forecasts
            var random = new Random();
            foreach (var city in cities)
            {
                for (int i = 0; i < 30; i++)
                {
                    var forecast = new WeatherForecast
                    {
                        Date = DateOnly.FromDateTime(DateTime.Today.AddDays(i)),
                        TemperatureC = random.Next(-10, 40), // Random temperatures between -10°C and 40°C
                        Summary = (WeatherSummary)random.Next(1, 11), // Random summary from 1 to 10
                        City = city
                    };

                    if (! await forecastsSet.AnyAsync(f => f.City.Name == city.Name && f.Date == forecast.Date))
                    {
                        await forecastsSet.AddAsync(forecast);
                    }
                }
            }

            await _dbcontext.SaveChangesAsync();
        }
    }
}
