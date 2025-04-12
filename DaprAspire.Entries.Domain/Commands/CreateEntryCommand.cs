using DaprAspire.Entries.Domain.Aggregates;
using EventFlow.Commands;

namespace DaprAspire.Entries.Domain.Commands
{
    public class CreateEntryCommand : Command<LedgerEntryAggregate, LedgerEntryId>
    {
        public CreateEntryCommand(LedgerEntryId aggregateId) : base(aggregateId) { }
    }
}
