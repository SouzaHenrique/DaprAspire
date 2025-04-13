using DaprAspire.Entries.Domain.Aggregates;

using EventFlow.Aggregates;

namespace DaprAspire.Entries.Domain.Events
{
    /// <summary>
    /// Represents an event that is emitted when a ledger entry is created.
    /// </summary>
    public class EntryCreatedEvent : AggregateEvent<LedgerEntryAggregate, LedgerEntryId>
    {
        public EntryCreatedEvent() { }
    }
}
