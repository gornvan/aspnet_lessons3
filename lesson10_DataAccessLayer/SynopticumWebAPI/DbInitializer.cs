using SynopticumDAL.Seed;
using SynopticumDAL;

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

            dbcontext.Database.EnsureCreated();

            var seeder = new SynopticumDbSeed(dbcontext);
            await seeder.Seed();
        }
    }
}
