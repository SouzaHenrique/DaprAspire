using DaprAspire.IdentityService.Domain.Entities.Identity;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DaprAspire.IdentityService.Infrastructure.Identity.Stores;
using MongoDB.Driver;
using DaprAspire.IdentityService.Application.Commom.Managers;
using DaprAspire.IdentityService.Application.Services.Seeders;
using DaprAspire.IdentityService.Application.HostedServices;
using DaprAspire.IdentityService.Infrastructure.Identity.Services;
using DaprAspire.IdentityService.Application.Services.Auth;

namespace DaprAspire.IdentityService.Application
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentityService();
            services.AddMongo(configuration);
            return services;
        }

        public static IServiceCollection AddIdentityService(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                    .AddUserStore<UserStore<ApplicationUser, ApplicationRole, string>>()
                    .AddRoleStore<RoleStore<ApplicationRole, string>>()
                    .AddUserManager<UserManager<ApplicationUser>>()
                    .AddRoleManager<RoleManager<ApplicationRole>>()
                    .AddSignInManager<SignInManager<ApplicationUser>>()
                    .AddDefaultTokenProviders();

            services.AddScoped<UserManager<ApplicationUser>, ApplicationUserManager>();
            services.AddScoped<RoleManager<ApplicationRole>, ApplicationRoleManager>();

            services.AddScoped<IdentitySeeder>();
            services.AddHostedService<IdentitySeederHostedService>();
            services.AddScoped<TokenGeneratorService>();
            services.AddScoped<AuthService>();

            return services;
        }

        public static IServiceCollection AddMongo(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IMongoClient>(_ =>
            new MongoClient(configuration.GetConnectionString("mongodb")));

            services.AddScoped(serviceProvider =>
                serviceProvider.GetRequiredService<IMongoClient>().GetDatabase("identitydb"));

            return services;
        }

    }
}
