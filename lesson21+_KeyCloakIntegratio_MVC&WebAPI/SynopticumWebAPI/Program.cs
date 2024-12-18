using SynopticumDAL.DBSeed;
using SynopticumWebAPI.Startup;

namespace lesson8_WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            SynopticumWebAPIModule.ConfigureServices(builder);

            var app = builder.Build();

            await DbInitializer.InitializeDb(app.Services);

            app.RegisterRoutes();

            app.InitializePipeline();
            app.ApplyApiVersionRoutePrefix();
            app.MapControllers();

            app.Run();
        }
    }
}
