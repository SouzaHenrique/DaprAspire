using DaprAspire.Entries.Domain.Aggregates;
using EventFlow.Commands;

namespace DaprAspire.Entries.Domain.Commands
{
    public class DebitEntryCommand : Command<LedgerEntryAggregate, LedgerEntryId>
    {
        public decimal Value { get; }
        public DebitEntryCommand(LedgerEntryId id, decimal value) : base(id) => Value = value;
    }
}
