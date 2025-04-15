using DaprAspire.Entries.Domain.ServiceDefinitions.Persistence;
using DaprAspire.Entries.Infrastructure.Persistence;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using MongoDB.Driver;

namespace DaprAspire.Entries.Infrastructure
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Add the MongoDB client to the service collection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IServiceCollection AddMongo(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton(sp =>
            {
                var mongoClient = new MongoClient(config.GetConnectionString("mongodb"));
                return mongoClient.GetDatabase("entries");
            });

            return services;
        }

        /// <summary>
        /// Add the repositories to the service collection.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ILedgerEntryReadModelRepository, LedgerEntryReadModelRepository>();
            return services;
        }
    }
}
