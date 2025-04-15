using DaprApire.Entries.Application.Features.Commands;
using DaprApire.Entries.Application.Features.Queries;
using DaprApire.Entries.Application.Services.PubSub;

using DaprAspire.Entries.Domain.Commands;
using DaprAspire.Entries.Domain.Events;
using DaprAspire.Entries.Domain.Queries;
using DaprAspire.Entries.Domain.ReadModels;

using EventFlow.Extensions;
using EventFlow.MongoDB.Extensions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DaprApire.Entries.Application
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Adds the EventFlow types and configurations for the Entries module.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddEntriesEventFlowTypes(this IServiceCollection services, IConfiguration configuration)
        {
            // Register the EventFlow modules and other dependencies here
            services.AddEventFlow(options =>
            {
                var mongodbConnectionString = configuration.GetConnectionString("mongodb");
                options.ConfigureMongoDb(url: mongodbConnectionString, "entries")
                .UseMongoDbEventStore()
                .UseMongoDbSnapshotStore()
                .UseMongoDbReadModel<LedgerEntryReadModel>()

                .AddEvents(typeof(EntryCreatedEvent), typeof(EntryCreditedEvent), typeof(EntryDebitedEvent))

                .AddCommands(typeof(CreateEntryCommand), typeof(CreditEntryCommand), typeof(DebitEntryCommand))

                .AddCommandHandlers(typeof(CreateEntryCommandHandler),
                                    typeof(CreditEntryCommandHandler),
                                    typeof(DebitEntryCommandHandler))

                .AddQueryHandler<GetAllLedgerEntriesQueryHandler, GetAllLedgerEntriesQuery, IReadOnlyCollection<LedgerEntryReadModel>>()

                .AddSubscribers(typeof(EntryCreatedEventPublisher),
                                typeof(EntryCreditedEventPublisher),
                                typeof(EntryDebitedEventPublisher));
            });
            return services;
        }
    }
}
