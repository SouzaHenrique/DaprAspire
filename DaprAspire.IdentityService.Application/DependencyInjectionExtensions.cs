using DaprAspire.IdentityService.Domain.Entities.Identity;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DaprAspire.IdentityService.Infrastructure.Identity.Stores;
using MongoDB.Driver;

namespace DaprAspire.IdentityService.Application
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddIdentityService(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                    .AddUserStore<UserStore<ApplicationUser, ApplicationRole, string>>()
                    .AddRoleStore<RoleStore<ApplicationRole, string>>()
                    .AddUserManager<UserManager<ApplicationUser>>()
                    .AddRoleManager<RoleManager<ApplicationRole>>()
                    .AddSignInManager<SignInManager<ApplicationUser>>()
                    .AddDefaultTokenProviders();

            return services;
        }

        public static IServiceCollection AddMongo(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IMongoClient>(_ =>
            new MongoClient(configuration.GetConnectionString("MongoDb")));

            services.AddScoped(serviceProvider =>
                serviceProvider.GetRequiredService<IMongoClient>().GetDatabase("identitydb"));

            return services;
        }
    }
}
