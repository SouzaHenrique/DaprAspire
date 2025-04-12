using DaprAspire.Entries.Domain.Commands;
using DaprAspire.Entries.Domain.Events;
using DaprAspire.Entries.Domain.ReadModels;
using EventFlow.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace DaprApire.Entries.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddEntriesEventFlowTypes(this IServiceCollection services)
        {
            // Register the EventFlow modules and other dependencies here
            services.AddEventFlow(options =>
            {
                options.AddEvents(typeof(EntryCreatedEvent), typeof(EntryCreditedEvent), typeof(EntryDebitedEvent));
                options.AddCommands(typeof(DebitEntryCommand), typeof(CreditEntryCommand), typeof(CreateEntryCommand));
                options.UseInMemoryReadStoreFor<LedgerEntryReadModel>();
            });
            return services;
        }
    }
}
