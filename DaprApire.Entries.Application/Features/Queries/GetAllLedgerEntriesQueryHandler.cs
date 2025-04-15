using DaprAspire.Entries.Domain.Queries;
using DaprAspire.Entries.Domain.ReadModels;
using DaprAspire.Entries.Domain.ServiceDefinitions.Persistence;

using EventFlow.Queries;

using Microsoft.Extensions.Logging;

namespace DaprApire.Entries.Application.Features.Queries
{
    internal class GetAllLedgerEntriesQueryHandler(
        ILedgerEntryReadModelRepository repository,
        ILogger<GetAllLedgerEntriesQueryHandler> logger)
        : IQueryHandler<GetAllLedgerEntriesQuery, IReadOnlyCollection<LedgerEntryReadModel>>
    {
        public async Task<IReadOnlyCollection<LedgerEntryReadModel>> ExecuteQueryAsync(GetAllLedgerEntriesQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("Fetching paginated ledger entries - Page: {Page}, PageSize: {PageSize}", query.Page, query.PageSize);

            var result = await repository.GetPagedAsync(query.Page, query.PageSize, cancellationToken);

            return result;
        }
    }
}
