using DaprAspire.Entries.Domain.Aggregates;
using DaprAspire.Entries.Domain.Events;
using EventFlow.Aggregates;
using EventFlow.ReadStores;

namespace DaprAspire.Entries.Domain.ReadModels
{
    /// <summary>
    /// Represents a read model for a ledger entry.
    /// </summary>
    public class LedgerEntryReadModel : IReadModel,
        IAmReadModelFor<LedgerEntryAggregate, LedgerEntryId, EntryCreatedEvent>,
        IAmReadModelFor<LedgerEntryAggregate, LedgerEntryId, EntryCreditedEvent>,
        IAmReadModelFor<LedgerEntryAggregate, LedgerEntryId, EntryDebitedEvent>

    {
        public string Id { get; set; } = default!;
        public decimal Balance { get; set; }

        /// <summary>
        /// Applies the EntryCreatedEvent to initialize the read model.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="domainEvent"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task ApplyAsync(IReadModelContext context, IDomainEvent<LedgerEntryAggregate, LedgerEntryId, EntryCreatedEvent> domainEvent, CancellationToken cancellationToken)
        {
            Id = domainEvent.AggregateIdentity.Value;
            Balance = 0;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Applies the EntryCreditedEvent to update the balance in the read model.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="domainEvent"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task ApplyAsync(IReadModelContext context, IDomainEvent<LedgerEntryAggregate, LedgerEntryId, EntryCreditedEvent> domainEvent, CancellationToken cancellationToken)
        {
            Balance += domainEvent.AggregateEvent.Value;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Applies the EntryDebitedEvent to update the balance in the read model.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="domainEvent"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task ApplyAsync(IReadModelContext context, IDomainEvent<LedgerEntryAggregate, LedgerEntryId, EntryDebitedEvent> domainEvent, CancellationToken cancellationToken)
        {
            Balance -= domainEvent.AggregateEvent.Value;
            return Task.CompletedTask;
        }       
    }
}
