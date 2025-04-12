using DaprAspire.Entries.Domain.Aggregates;
using EventFlow.Commands;

namespace DaprAspire.Entries.Domain.Commands
{
    public class CreditEntryCommand : Command<LedgerEntryAggregate, LedgerEntryId>
    {
        public decimal Value { get; }
        public CreditEntryCommand(LedgerEntryId id, decimal value) : base(id) => Value = value;
    }
}
