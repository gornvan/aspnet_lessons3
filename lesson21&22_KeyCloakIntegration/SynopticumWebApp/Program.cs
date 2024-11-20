using SynopticumDAL.DBSeed;

namespace SynopticumWebApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            SynopticumWebAppModule.ConfigureServices(builder);

            var app = builder.Build();

            await DbInitializer.InitializeDb(app.Services);

            BuildRequestPipeline(app);

            MapRoutes(app);

            app.Run();
        }

        private static void MapRoutes(WebApplication app)
        {
            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}", new { area = "Home" });

            app.MapRazorPages();
        }

        private static void BuildRequestPipeline(WebApplication app)
        {

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}
