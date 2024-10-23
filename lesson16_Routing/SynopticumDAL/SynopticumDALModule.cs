using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SynopticumDAL.Contract;
using SynopticumDAL.Services;

namespace SynopticumDAL
{
    public static class SynopticumDALModule
    {
        public static void RegisterModule(IServiceCollection services, IConfiguration configuration, bool isDevelopment)
        {
            var connectionString = configuration.GetConnectionString("Default");
            var serverVersion = configuration.GetSection("MySql").GetValue<string>("Version");

            services.AddDbContext<SynopticumDbContext>(
                dbOptionsBuilder => ConfigureDbOptionsBuilderForMySql(
                    dbOptionsBuilder,
                    connectionString,
                    serverVersion,
                    isDevelopment
                    )
            );

            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void ConfigureDbOptionsBuilderForMySql(
            DbContextOptionsBuilder builder,
            string connectionString,
            string mySqlServerVersion,
            bool isDevelopment)
        {
            builder
                .UseMySql(connectionString, ServerVersion.Parse(mySqlServerVersion))
                // The following three options help with debugging, but should
                // be changed or removed for production.
                .LogTo(Console.WriteLine, LogLevel.Warning);
            if (isDevelopment)
            {
                builder
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
            }
        }
    }
}
