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
        /// <summary>
        /// The current balance of the ledger entry.
        /// </summary>
        private decimal _balance;

        /// <summary>
        /// Gets the current balance of the ledger entry.
        /// </summary>
        public decimal Balance => _balance;

        /// <summary>
        /// Initializes a new instance of the <see cref="LedgerEntryAggregate"/> class.
        /// </summary>
        /// <param name="id"></param>
        public LedgerEntryAggregate(LedgerEntryId id) : base(id) { }

        /// <summary>
        /// Creates a new ledger entry.
        /// </summary>
        public void Create() => Emit(new EntryCreatedEvent());

        /// <summary>
        /// Credits the ledger entry with the specified value.
        /// </summary>
        /// <param name="valor"></param>
        public void Credit(decimal valor) => Emit(new EntryCreditedEvent(valor));

        /// <summary>
        /// Debits the ledger entry with the specified value.
        /// </summary>
        /// <param name="valor"></param>
        public void Debit(decimal valor) => Emit(new EntryDebitedEvent(valor));

        /// <summary>
        /// Applies the <see cref="EntryCreatedEvent"/> to the aggregate.
        /// </summary>
        /// <param name="_"></param>
        public void Apply(EntryCreatedEvent _) => _balance = 0;

        /// <summary>
        /// Applies the <see cref="EntryCreditedEvent"/> to the aggregate.
        /// </summary>
        /// <param name="e"></param>
        public void Apply(EntryCreditedEvent e) => _balance += e.Value;

        /// <summary>
        /// Applies the <see cref="EntryDebitedEvent"/> to the aggregate.
        /// </summary>
        /// <param name="e"></param>
        public void Apply(EntryDebitedEvent e) => _balance -= e.Value;
    }
}
