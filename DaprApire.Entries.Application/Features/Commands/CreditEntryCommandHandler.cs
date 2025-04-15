using DaprAspire.Entries.Domain.Aggregates;
using DaprAspire.Entries.Domain.Commands;
using EventFlow.Commands;

using Microsoft.Extensions.Logging;

namespace DaprApire.Entries.Application.Features.Commands
{
    /// <summary>
    /// Handles the command to credit an entry in the ledger.
    /// </summary>
    public class CreditEntryCommandHandler(ILogger<CreditEntryCommandHandler> logger) : CommandHandler<LedgerEntryAggregate, LedgerEntryId, CreditEntryCommand>
    {
        private readonly ILogger<CreditEntryCommandHandler> _logger = logger;

        public override async Task ExecuteAsync(LedgerEntryAggregate aggregate, CreditEntryCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Crediting ledger entry with ID: {Id}", command.AggregateId.Value);
            aggregate.Credit(command.Value);
            await Task.CompletedTask;
        }
    }
}
