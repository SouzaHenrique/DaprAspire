using DaprAspire.Entries.Domain.Aggregates;

using EventFlow.Aggregates;

namespace DaprAspire.Entries.Domain.Events
{
    public class EntryCreatedEvent : AggregateEvent<LedgerEntryAggregate, LedgerEntryId>
    {
        public EntryCreatedEvent() { }
    }
}
