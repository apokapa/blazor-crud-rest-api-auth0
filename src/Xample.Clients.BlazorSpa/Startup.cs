using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Xample.Clients.BlazorSpa
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped((sp) =>
            {
                return new Blazor.Auth0.Shared.Models.ClientSettings()
                {
                    Auth0Domain = "Auth0Domain",
                    Auth0ClientId = "Auth0ClientId",
                    //// Redirection to home can be forced uncommenting the following line, this setting primes over Auth0RedirectUri
                    //// RedirectAlwaysToHome = true,
                    //// Uncomment following line to force the user to be authenticated
                    //// LoginRequired = true
                    Auth0Audience = "api-audience"
                };
            });

            services.AddScoped<Blazor.Auth0.ClientSide.Authentication.AuthenticationService>();
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
