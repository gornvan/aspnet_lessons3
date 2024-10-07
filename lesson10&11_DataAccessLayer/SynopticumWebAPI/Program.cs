
using lesson8_WebApi.Middlewares;
using SynopticumCore;
using SynopticumDAL;
using SynopticumDAL.Seed;
using SynopticumWebAPI;
using System.Reflection;

namespace lesson8_WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

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

            var app = builder.Build();

            await DbInitializer.InitializeDb(app.Services);

            ApplyApiVersionRoutePrefix(app);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseHttpLogging();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.UseResponseCompression();
            app.Run();
        }

        private static void ApplyApiVersionRoutePrefix(WebApplication app)
        {
            var appVersion = Assembly.GetExecutingAssembly().GetName().Version;
            var pathPrefix = $"/api/{appVersion}";
            app.UseMiddleware<GlobalRoutePrefixMiddleware>(pathPrefix);
            app.UsePathBase(pathPrefix);
        }
    }
}
