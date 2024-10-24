﻿using lesson8_WebApi.Middlewares;
using SynopticumWebAPI.Middlewares;
using System.Reflection;

namespace SynopticumWebAPI.Startup;

public static class ApplicationPipeline
{
    public static void InitializePipeline(this WebApplication app)
    {
        app.UseResponseCompression();

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

        app.UseMiddleware<BadRequestLoggingMiddleware>();
    }

    private static void ApplyApiVersionRoutePrefix(WebApplication app)
    {
        var appVersion = Assembly.GetExecutingAssembly().GetName().Version;
        var pathPrefix = $"/api/{appVersion}";
        app.UseMiddleware<GlobalRoutePrefixMiddleware>(pathPrefix);
        app.UsePathBase(pathPrefix);
    }
}
