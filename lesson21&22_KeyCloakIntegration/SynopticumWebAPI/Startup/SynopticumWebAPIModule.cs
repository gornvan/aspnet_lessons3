using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using SynopticumCore;
using SynopticumCore.Contract;
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
                AddSwagger(builder);
                // Add services to the container.
                builder.Services.AddHttpLogging(o => { });
            }

            AddSerilog(builder);
            AddAuth(builder);

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

        private static void AddSwagger(WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

                // Add support for JWT Bearer Authorization in Swagger UI
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });
        }

        private static void AddAuth(WebApplicationBuilder builder)
        {
            var tokenValidationParams = builder.Configuration
                .GetSection(nameof(TokenValidationParameters))
                .Get<TokenValidationParametersConfig>();
            if (tokenValidationParams == null)
            {
                throw new SystemException("Could not parse TokenValidationParams in settings!");
            }

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = tokenValidationParams.Authority;

                    options.TokenValidationParameters = new TokenValidationParameters {
                        ValidateIssuer = tokenValidationParams.ValidateIssuer,
                        ValidIssuer = tokenValidationParams.ValidIssuer,
                        ValidateAudience = tokenValidationParams.ValidateAudience,
                        ValidAudience = tokenValidationParams.ValidAudience,
                        ValidateLifetime = tokenValidationParams.ValidateLifetime,
                        ValidateIssuerSigningKey = tokenValidationParams.ValidateIssuerSigningKey,
                    };

                    options.RequireHttpsMetadata = true;
                    options.SaveToken = false;

                    if (builder.Environment.IsDevelopment())
                    {
                        // Optional: Debugging and HTTPS enforcement
                        options.RequireHttpsMetadata = false; // Set to true in production
                        options.SaveToken = true; // Save token for debugging

                        options.Events = new JwtBearerEvents
                        {
                            OnAuthenticationFailed = context =>
                            {
                                Console.WriteLine("Authentication failed: " + context.Exception.Message);
                                return Task.CompletedTask;
                            },
                            OnChallenge = context =>
                            {
                                Console.WriteLine("OnChallenge: " + context.ErrorDescription);
                                if (context.AuthenticateFailure != null)
                                {
                                    Console.WriteLine("Authentication failure: " + context.AuthenticateFailure.Message);
                                }
                                return Task.CompletedTask;
                            },
                            OnTokenValidated = context =>
                            {
                                Console.WriteLine("Token validated successfully");
                                return Task.CompletedTask;
                            }
                        };
                    }
                });

            // Enable authorization
            builder.Services.AddAuthorization();
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
                builder.Logging.SetMinimumLevel(LogLevel.Debug);
                builder.Logging.AddConsole();
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
