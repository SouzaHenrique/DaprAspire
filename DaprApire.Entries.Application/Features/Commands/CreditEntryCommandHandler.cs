using DaprAspire.Entries.Domain.Aggregates;
using DaprAspire.Entries.Domain.Commands;
using EventFlow.Commands;

namespace DaprApire.Entries.Application.Features.Commands
{
    /// <summary>
    /// Handles the command to credit an entry in the ledger.
    /// </summary>
    public class CreditEntryCommandHandler : CommandHandler<LedgerEntryAggregate, LedgerEntryId, CreditEntryCommand>
    {
        public override async Task ExecuteAsync(LedgerEntryAggregate aggregate, CreditEntryCommand command, CancellationToken cancellationToken)
        {
            aggregate.Credit(command.Value);
            await Task.CompletedTask;
        }
    }
}
