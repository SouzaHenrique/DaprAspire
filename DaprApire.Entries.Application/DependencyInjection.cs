using DaprApire.Entries.Application.Features.Commands;

using DaprAspire.Entries.Domain.Commands;
using DaprAspire.Entries.Domain.Events;
using DaprAspire.Entries.Domain.ReadModels;

using EventFlow.Extensions;
using EventFlow.MongoDB.Extensions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DaprApire.Entries.Application
{
    public static class DependencyInjection
    {
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
                                    typeof(DebitEntryCommandHandler));
            });
            return services;
        }
    }
}
