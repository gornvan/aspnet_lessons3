using Microsoft.EntityFrameworkCore;
using SynopticumDAL;

namespace SynopticumWebAPI
{
    public static class DALConfigurer
    {
        public static void Configure(IServiceCollection services, IConfiguration configuration, bool isDevelopment)
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
        }
    }
}
