using DaprAspire.Entries.Domain.Aggregates;

using EventFlow.Aggregates;

namespace DaprAspire.Entries.Domain.Events
{
    /// <summary>
    /// Represents an event that occurs when a ledger entry is debited.
    /// </summary>
    public class EntryDebitedEvent : AggregateEvent<LedgerEntryAggregate, LedgerEntryId>
    {
        public decimal Value { get; }
        public EntryDebitedEvent(decimal value) => Value = value;
    }
}
