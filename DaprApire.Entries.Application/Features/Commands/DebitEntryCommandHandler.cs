using DaprAspire.Entries.Domain.Aggregates;
using DaprAspire.Entries.Domain.Commands;

using EventFlow.Commands;

using Microsoft.Extensions.Logging;

namespace DaprApire.Entries.Application.Features.Commands
{
    /// <summary>
    /// Handles the debit operation for a ledger entry.
    /// </summary>
    public class DebitEntryCommandHandler(ILogger<CreditEntryCommandHandler> logger) : CommandHandler<LedgerEntryAggregate, LedgerEntryId, DebitEntryCommand>
    {
        private readonly ILogger<CreditEntryCommandHandler> _logger = logger;

        public override async Task ExecuteAsync(LedgerEntryAggregate aggregate, DebitEntryCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Debiting ledger entry with ID: {Id}", command.AggregateId.Value);
            aggregate.Debit(command.Value);
            await Task.CompletedTask;
        }
    }
}
