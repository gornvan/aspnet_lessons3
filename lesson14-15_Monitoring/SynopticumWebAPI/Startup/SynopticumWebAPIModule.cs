using Serilog;
using SynopticumCore;
using SynopticumDAL;
using SynopticumWebAPI.ConfigurationSections;
using ILogger = Serilog.ILogger;

namespace SynopticumWebAPI.Startup
{
    public static class SynopticumWebAPIModule
    {
        public static void ConfigureServices(WebApplicationBuilder builder)
        {
            if (builder.Environment.IsDevelopment())
            {
                // Add services to the container.
                builder.Services.AddHttpLogging(o => { });
            }

            AddSerilog(builder);

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            SynopticumDALModule.RegisterModule(
                builder.Services,
                builder.Configuration,
                builder.Environment.IsDevelopment());

            builder.Services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
            });

            SynopticumCoreModule.RegisterModule(builder.Services);
        }

        internal static void AddSerilog(WebApplicationBuilder builder)
        {
            var serilogConfig = builder.Configuration.GetSection(nameof(SerilogConfig)).Get<SerilogConfig>();
            var logFilePath = Path.Combine(serilogConfig?.LoggingDir ?? "./", "log.txt");

            var loggerConfig = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File(
                    logFilePath,
                    rollingInterval: RollingInterval.Month,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}");

            if (builder.Environment.IsDevelopment())
            {
                loggerConfig = loggerConfig.MinimumLevel.Debug();
            }
            else
            {
                loggerConfig = loggerConfig.MinimumLevel.Warning();
            }

            var logger = loggerConfig.CreateLogger();

            builder.Services.AddSingleton<ILogger>(logger);
        }
    }
}
