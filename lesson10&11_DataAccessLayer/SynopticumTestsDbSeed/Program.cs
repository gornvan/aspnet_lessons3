using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SynopticumCoreTests;
using SynopticumDAL;
using SynopticumDAL.Seed;
using SynopticumDAL.Services;

namespace SynopticumTestsDbSeed
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var dbContext = DbContextInitializer.Initialize("seedsettings.json");

            dbContext.Database.EnsureCreated();

            var seeder = new SynopticumDbSeed(dbContext);
            await seeder.Seed();

            Console.WriteLine($@"The DB {dbContext.Database.GetDbConnection().Database} has been seeded successfully!");
        }
    }
}
