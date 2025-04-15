using DaprAspire.Entries.Domain.Aggregates;
using DaprAspire.Entries.Domain.Commands;

using EventFlow.Commands;

using Microsoft.Extensions.Logging;

namespace DaprApire.Entries.Application.Features.Commands
{
    /// <summary>
    /// Handles the creation of a ledger entry.
    /// </summary>
    public class CreateEntryCommandHandler(ILogger<CreateEntryCommandHandler> logger) : CommandHandler<LedgerEntryAggregate, LedgerEntryId, CreateEntryCommand>
    {
        ILogger<CreateEntryCommandHandler> _logger = logger;

        public override async Task ExecuteAsync(LedgerEntryAggregate aggregate, CreateEntryCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating a new ledger entry with ID: {Id}", command.AggregateId.Value);
            aggregate.Create();
            await Task.CompletedTask;
        }
    }
}
