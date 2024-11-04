using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SynopticumIdentityServer.Client;

namespace SynopticumIdentityServer.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.Services.AddAuthorizationCore();
            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

            await builder.Build().RunAsync();
        }
    }
}
