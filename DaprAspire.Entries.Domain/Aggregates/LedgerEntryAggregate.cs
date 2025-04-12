using DaprAspire.Entries.Domain.Events;

using EventFlow.Aggregates;

namespace DaprAspire.Entries.Domain.Aggregates
{
    /// <summary>
    /// Represents a ledger entry aggregate.
    /// </summary>
    public class LedgerEntryAggregate : AggregateRoot<LedgerEntryAggregate, LedgerEntryId>,
        IEmit<EntryCreatedEvent>,
        IEmit<EntryCreditedEvent>,
        IEmit<EntryDebitedEvent>
    {
        private decimal _balance;

        public LedgerEntryAggregate(LedgerEntryId id) : base(id) { }

        public void Create() => Emit(new EntryCreatedEvent());
        public void Credit(decimal valor) => Emit(new EntryCreditedEvent(valor));
        public void Debit(decimal valor) => Emit(new EntryDebitedEvent(valor));


        public void Apply(EntryCreatedEvent _) => _balance = 0;
        public void Apply(EntryCreditedEvent e) => _balance += e.Value;
        public void Apply(EntryDebitedEvent e) => _balance -= e.Value;
    }
}
