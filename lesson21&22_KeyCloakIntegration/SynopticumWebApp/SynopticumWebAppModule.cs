using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using SynopticumCore;
using SynopticumDAL;

namespace SynopticumWebApp
{
    public class SynopticumWebAppModule
    {
        public static void ConfigureServices(WebApplicationBuilder builder)
        {
            SynopticumDALModule.RegisterModule(
                builder.Services,
                builder.Configuration,
                builder.Environment.IsDevelopment());

            SynopticumCoreModule.RegisterModule(builder.Services);

            AddKeyCloakAuth(builder);
            AddGoogleAuth(builder);
            ConfigureAuth(builder);

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
        }

        private static void ConfigureAuth(WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            });

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            });
        }

        private static void AddGoogleAuth(WebApplicationBuilder builder)
        {
            var clientId = builder.Configuration["Authentication:Google:ClientId"];
            var clientSecret = builder.Configuration["Authentication:Google:ClientSecret"];

            builder.Services.AddAuthentication()
                .AddGoogle("Google",
                googleOptions =>
                {
                    googleOptions.ClientId = clientId;
                    googleOptions.ClientSecret = clientSecret;
                });
        }

        private static void AddKeyCloakAuth(WebApplicationBuilder builder)
        {
            var keycloakSettingsSection = builder.Configuration.GetSection(KeycloakAuthenticationOptions.Section);

            builder.Services.AddAuthentication()
                .AddKeycloakWebApp(
                    keycloakSettingsSection,
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


            builder.Services.AddKeycloakAuthorization(builder.Configuration);

        }
    }
}
