using lesson8_WebApi.Middlewares;
using System.Reflection;

namespace SynopticumWebAPI.Startup
{
    public static class Routing
    {
        public static void RegisterRoutes(this WebApplication app)
        {
            app.MapControllerRoute(
                "probes",
                "probes/{coordinates}/{probeType}",
                new {
                    Controller = "Probes",
                    Action = "GetTemperature",
                    coordinates = "53.9,27.55",
                });

            app.MapControllerRoute(
                "default",
                "{Controller}/{Action}");
        }

        public static void ApplyApiVersionRoutePrefix(this WebApplication app)
        {
            var appVersion = Assembly.GetExecutingAssembly().GetName().Version;
            var pathPrefix = $"/api/{appVersion}";
            app.UseMiddleware<GlobalRoutePrefixMiddleware>(pathPrefix);
            app.UsePathBase(pathPrefix);
        }
    }
}
