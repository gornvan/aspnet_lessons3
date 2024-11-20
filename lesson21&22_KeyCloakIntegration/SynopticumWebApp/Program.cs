using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using SynopticumWebApp.Data;
using SynopticumWebApp.Data.Entities;

namespace SynopticumWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            AddDatabase(builder);

            ConfigureAuth(builder);

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

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

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}", new { area = "Home" });

            app.MapRazorPages();

            app.Run();
        }

        private static void ConfigureAuth(WebApplicationBuilder builder)
        {
            builder.Services.AddDefaultIdentity<SynopticumUser>(
                options =>
                {
                    options.SignIn.RequireConfirmedAccount = true;
                }
            ).AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddAuthentication()
                .AddKeycloakWebApp(
                    builder.Configuration.GetSection(KeycloakAuthenticationOptions.Section),
                    configureOpenIdConnectOptions: options =>
                    {
                        // we need this for front-channel sign-out
                        options.SaveTokens = true;
                        options.ResponseType = OpenIdConnectResponseType.Code;
                        options.Events = new OpenIdConnectEvents
                        {
                            OnSignedOutCallbackRedirect = context =>
                            {
                                context.Response.Redirect("/Home/Public");
                                context.HandleResponse();

                                return Task.CompletedTask;
                            },
                            // we can react to successful authentication here
                            //OnAuthorizationCodeReceived = context =>
                            //{
                            //    return Task.CompletedTask;
                            //}
                        };
                    },
                    openIdConnectScheme: "KeyCloak"
                );
            builder.Services.AddAuthentication()
                .AddGoogle("Google",
                googleOptions =>
                {
                    googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                    googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
                });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            });

            builder.Services.AddKeycloakAuthorization(builder.Configuration);


            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            });
        }

        public static void AddDatabase(WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration
                .GetConnectionString("Default") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            var mySqlVersion = builder.Configuration
                .GetSection("MySql").GetValue<string>("Version");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.Parse(mySqlVersion)));
        }
    }
}
