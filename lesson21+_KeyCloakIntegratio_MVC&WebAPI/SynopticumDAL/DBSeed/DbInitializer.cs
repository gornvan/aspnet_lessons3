using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SynopticumDAL.Services;
using SynopticumWebApp.Data.Entities;

namespace SynopticumDAL.DBSeed
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

            await InitializeTestingData(initializationScope);

            await InitializeIdentity(initializationScope);
        }

        private static async Task InitializeIdentity(IServiceScope initializationScope)
        {
            var userManager = initializationScope.ServiceProvider.GetRequiredService<UserManager<SynopticumUser>>();
            var roleManager = initializationScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var seeder = new SynopticumIdentityDbSeed(userManager, roleManager);

            await seeder.Seed();
        }

        private static async Task InitializeTestingData(IServiceScope initializationScope)
        {
            var dbcontext = initializationScope.ServiceProvider.GetRequiredService<SynopticumDbContext>();

            dbcontext.Database.Migrate();

            var seeder = new SynopticumTestsDbSeed(dbcontext);
            await seeder.Seed();
        }
    }
}
