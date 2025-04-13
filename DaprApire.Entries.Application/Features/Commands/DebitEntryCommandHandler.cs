using DaprAspire.Entries.Domain.Aggregates;
using DaprAspire.Entries.Domain.Commands;

using EventFlow.Commands;

namespace DaprApire.Entries.Application.Features.Commands
{
    /// <summary>
    /// Handles the debit operation for a ledger entry.
    /// </summary>
    public class DebitEntryCommandHandler : CommandHandler<LedgerEntryAggregate, LedgerEntryId, DebitEntryCommand>
    {
        public override async Task ExecuteAsync(LedgerEntryAggregate aggregate, DebitEntryCommand command, CancellationToken cancellationToken)
        {
            aggregate.Debit(command.Value);
            await Task.CompletedTask;
        }
    }
}
