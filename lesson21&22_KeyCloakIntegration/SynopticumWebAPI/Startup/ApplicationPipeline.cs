using lesson8_WebApi.Middlewares;
using SynopticumWebAPI.Middlewares;
using System.Reflection;

namespace SynopticumWebAPI.Startup;

public static class ApplicationPipeline
{
    public static void InitializePipeline(this WebApplication app)
    {
        app.UseResponseCompression();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseHttpLogging();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseMiddleware<BadRequestLoggingMiddleware>();
    }
}
