using SynopticumCore;
using SynopticumDAL;

namespace SynopticumWebAPI
{
    public static class SynopticumWebAPIModule
    {
        public static void ConfigureServices(WebApplicationBuilder builder)
        {
            // Add services to the container.
            builder.Services.AddHttpLogging(o => { });

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
    }
}
