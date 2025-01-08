using Microsoft.EntityFrameworkCore;
using SynopticumCoreTests;

namespace SynopticumTestsDbSeed
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var dbContext = DbContextInitializer.Initialize("seedsettings.json");

            dbContext.Database.EnsureCreated();

            var seeder = new SynopticumDAL.DBSeed.SynopticumTestsDbSeed(dbContext);
            await seeder.Seed();

            Console.WriteLine($@"The DB {dbContext.Database.GetDbConnection().Database} has been seeded successfully!");
        }
    }
}
