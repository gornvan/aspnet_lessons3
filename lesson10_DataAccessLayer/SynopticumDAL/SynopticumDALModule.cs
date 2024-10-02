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
                dbContextOptions => {
                    dbContextOptions
                        .UseMySql(connectionString, ServerVersion.Parse(serverVersion))
                        // The following three options help with debugging, but should
                        // be changed or removed for production.
                        .LogTo(Console.WriteLine, LogLevel.Warning);
                    if (isDevelopment)
                    {
                        dbContextOptions
                        .EnableSensitiveDataLogging()
                        .EnableDetailedErrors();
                    }
                }
            );

            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
