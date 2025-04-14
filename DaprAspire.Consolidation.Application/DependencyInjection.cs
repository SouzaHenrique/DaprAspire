using DaprAspire.Consolidation.Application.Services.ProjectionService;
using DaprAspire.Consolidation.Infrastructure.Repositories;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using MongoDB.Driver;

namespace DaprAspire.Consolidation.Application
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Add the repositories to the service collection.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton(typeof(IProjectionRepository<>), typeof(MongoProjectionRepository<>));
            return services;
        }

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
                return mongoClient.GetDatabase("consolidation");
            });

            return services;
        }

        /// <summary>
        /// Add the projection services to the service collection.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddProjectionServices(this IServiceCollection services)
        {
            services.AddScoped<IEntryProjectionService, EntryProjectionService>();
            return services;
        }
    }
}
