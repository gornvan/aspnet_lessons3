using Microsoft.EntityFrameworkCore;
using SynopticumDAL.Services;
using SynopticumTestsDbSeed;

namespace SynopticumWebAPI
{
    public static class DbInitializer
    {
        /// <summary>
        /// expects SynopticumDbContext to be available already
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static async Task InitializeDb(IServiceProvider services)
        {
            using var initializationScope = services.CreateScope();

            var dbcontext = initializationScope.ServiceProvider.GetRequiredService<SynopticumDbContext>();

            dbcontext.Database.Migrate();

            var seeder = new SynopticumDbSeed(dbcontext);
            await seeder.Seed();
        }
    }
}
