using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SynopticumDAL;
using SynopticumDAL.Services;

namespace SynopticumCoreTests
{
    public static class DbContextInitializer
    {
        public static SynopticumDbContext Initialize(string configPath)
        {
            // create config builder and tell it to read configuration file
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddJsonFile(configPath);
            var config = configBuilder.Build();

            // read the settings
            var connectionString = config.GetConnectionString("Default");
            var serverVersion = config.GetSection("MySql").GetValue<string>("Version");

            // create dbcontext
            var dbOptionsBuilder = new DbContextOptionsBuilder<SynopticumDbContext>();
            SynopticumDALModule.ConfigureDbOptionsBuilderForMySql(
                dbOptionsBuilder,
                connectionString,
                serverVersion,
                true);
            var dbOptions = dbOptionsBuilder.Options;

            var dbContext = new SynopticumDbContext(dbOptions);

            return dbContext;
        }
    }
}
