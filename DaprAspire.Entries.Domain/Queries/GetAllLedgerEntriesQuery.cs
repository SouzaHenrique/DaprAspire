using DaprAspire.Entries.Domain.ReadModels;

using EventFlow.Queries;

namespace DaprAspire.Entries.Domain.Queries
{
    public class GetAllLedgerEntriesQuery(int page, int pageSize) : IQuery<IReadOnlyCollection<LedgerEntryReadModel>>
    {
        public int Page { get; } = page;
        public int PageSize { get; } = pageSize;
    }
}
