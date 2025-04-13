using DaprAspire.Entries.Domain.Aggregates;
using DaprAspire.Entries.Domain.Commands;

using EventFlow.Commands;

namespace DaprApire.Entries.Application.Features.Commands
{
    /// <summary>
    /// Handles the creation of a ledger entry.
    /// </summary>
    public class CreateEntryCommandHandler : CommandHandler<LedgerEntryAggregate, LedgerEntryId, CreateEntryCommand>
    {
        public override async Task ExecuteAsync(LedgerEntryAggregate aggregate, CreateEntryCommand command, CancellationToken cancellationToken)
        {
            aggregate.Create();
            await Task.CompletedTask;
        }
    }
}
