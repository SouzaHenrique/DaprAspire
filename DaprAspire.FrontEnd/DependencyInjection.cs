using DaprAspire.FrontEnd.Services.Auth;
using DaprAspire.FrontEnd.Services.Consolidations;
using DaprAspire.FrontEnd.Services.Entries;
using DaprAspire.FrontEnd.Services.Handlers;

namespace DaprAspire.FrontEnd
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddBackEndServices(this IServiceCollection services)
        {
            services.AddHttpClient("PublicClient", client =>
            {
                client.BaseAddress = new Uri("http://localhost:5055");
            });

            services.AddScoped<AuthService>();
            services.AddScoped<AuthorizationMessageHandler>();

            services.AddHttpClient("AuthorizedClient", client =>
            {
                client.BaseAddress = new Uri("http://localhost:5055/");
            }).AddHttpMessageHandler<AuthorizationMessageHandler>();

            services.AddScoped<EntriesService>();
            services.AddScoped<ConsolidationService>();

            return services;
        }
    }
}
