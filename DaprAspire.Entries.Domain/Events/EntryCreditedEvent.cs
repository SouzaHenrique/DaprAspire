using DaprAspire.Entries.Domain.Aggregates;

using EventFlow.Aggregates;

namespace DaprAspire.Entries.Domain.Events
{
    /// <summary>
    /// Represents an event that occurs when a ledger entry is credited.
    /// </summary>
    public class EntryCreditedEvent : AggregateEvent<LedgerEntryAggregate, LedgerEntryId>
    {
        public decimal Value { get; }
        public EntryCreditedEvent(decimal value) => Value = value;
    }
}
