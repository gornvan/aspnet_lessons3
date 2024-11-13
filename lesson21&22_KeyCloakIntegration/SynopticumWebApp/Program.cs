using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using SynopticumWebApp.Data;

namespace SynopticumWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            AddDatabase(builder);

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();


            builder.Services.AddKeycloakAuthorization(builder.Configuration);
            builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
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
                    }
                );

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
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
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
